using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KsFetch.Contracts.V1;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace KsFetch.GUI
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            Load += Form1_Load;
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _cancellation.Cancel();
            _currentTask.Wait();
            FinishWork();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (lstCategories.CheckedItems.Count <= 0)
            {
                Info("请选择至少一项。");
                return;
            }

            btnStop.Enabled = true;
            btnStart.Enabled = false;
            lstCategories.Enabled = false;
            txtEmail.Enabled = false;
            txtPassword.Enabled = false;
            cboSortMethod.Enabled = false;

            var dialogResult = dlgSaveFile.ShowDialog();
            if (dialogResult == DialogResult.Cancel || dlgSaveFile.FileName.Length <= 0)
            {
                txtEmail.Enabled = true;
                txtPassword.Enabled = true;
                FinishWork();
                return;
            }
            else
            {
                if (File.Exists(dlgSaveFile.FileName))
                {
                    dialogResult = MsgBox($"文件 {dlgSaveFile.FileName} 已存在，如果继续将会清除其内容，此操作不可恢复。确认继续吗？", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.No)
                    {
                        txtEmail.Enabled = true;
                        txtPassword.Enabled = true;
                        FinishWork();
                        return;
                    }
                }
                txtDatabaseLocation.Text = dlgSaveFile.FileName;
                _dbClient.Open(dlgSaveFile.FileName);
                _dbClient.CreateTable();
                _cancellation = new CancellationTokenSource();
                _currentTask = Task.Run(new Action(Work), _cancellation.Token);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            SetProgressTitle(-1);

            var values = Enum.GetValues(typeof(CategoryKind));
            values.ForEach((value) =>
            {
                switch ((CategoryKind)value)
                {
                    case CategoryKind.Recommended:
                    case CategoryKind.Starred:
                    case CategoryKind.Everything:
                        break;
                    default:
                        lstCategories.Items.Add(ChineseDescriptionAttribute.ExtractFromEnum((CategoryKind)value));
                        break;
                }
            });
            values = Enum.GetValues(typeof(SortMethod));
            values.ForEach((value) =>
            {
                cboSortMethod.Items.Add(ChineseDescriptionAttribute.ExtractFromEnum((SortMethod)value));
            });
            cboSortMethod.SelectedIndex = 1;
        }

        private void Work()
        {
            string logMessage;
            var logger = LoggerFactory.GetLogger(GetType());

            logMessage = "抓取开始。";
            Info(logMessage);
            logger.Info(logMessage);
            var sortMethod = mapIndexToSortMethod(cboSortMethod.SelectedIndex);

            if (!_webClient.IsLoggedIn)
            {
                logMessage = "正在登录...";
                Info(logMessage);
                logger.Info(logMessage);
                try
                {
                    _webClient.Login(txtEmail.Text, txtPassword.Text);
                }
                catch (KickstarterApiException ex)
                {
                    MsgBox(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logMessage = $"Kickstarter: {ex.Message}";
                    Info(logMessage);
                    logger.Error(logMessage, ex);
                    txtEmail.Enabled = true;
                    txtPassword.Enabled = true;
                    FinishWork();
                    return;
                }
                catch (Exception ex)
                {
                    logMessage = $"Unknown: {ex.Message}";
                    Info(logMessage);
                    logger.Error(logMessage, ex);
                    txtEmail.Enabled = true;
                    txtPassword.Enabled = true;
                    FinishWork();
                    return;
                }
            }

            // Access summary
            logMessage = "已登录。正在查询所有项目...";
            Info(logMessage);
            logger.Info(logMessage);
            var projectsSummary = _webClient.SearchInCategory(CategoryKind.Everything, sortMethod, 1);
            var totalProjectsCount = projectsSummary.TotalHits;
            var totalNeedToCheckCount = 0;
            SearchResponse searchResponse;
            for (var i = 0; i < lstCategories.CheckedIndices.Count; ++i)
            {
                logMessage = $"正在查询项目 {i + 1}/{lstCategories.CheckedIndices.Count} ({lstCategories.Items[lstCategories.CheckedIndices[i]]})...";
                Info(logMessage);
                logger.Info(logMessage);
                searchResponse = _webClient.SearchInCategory(mapIndexToCategory(lstCategories.CheckedIndices[i]), sortMethod, 1);
                totalNeedToCheckCount += (int)searchResponse.TotalHits;
                logMessage = $"{lstCategories.Items[lstCategories.CheckedIndices[i]]} 类有 {searchResponse.TotalHits} 个项目。";
                Info(logMessage);
                logger.Info(logMessage);
            }
            logMessage = $"总共 {totalProjectsCount} 个项目，选择的类别中包含 {totalNeedToCheckCount} 个。";
            Info(logMessage);
            logger.Info(logMessage);
            pbProgress.Value = 0;
            pbProgress.Maximum = totalNeedToCheckCount;
            lblProgressText.Text = string.Empty;

            // Calculate project data
            uint currentPageIndex;
            var currentProjectIndexOfAll = 0;
            int currentProjectIndexInPage;
            Project project;
            bool b;
            int errorCounter;
            const int ERROR_LIMIT = 20;
            double percent;
            for (var i = 0; i < lstCategories.CheckedIndices.Count; ++i)
            {
                logMessage = $"开始浏览类别 #{i + 1}/{lstCategories.CheckedIndices.Count} (ID={mapIndexToCategory(lstCategories.CheckedIndices[i])}) {lstCategories.Items[lstCategories.CheckedIndices[i]]}...";
                Info(logMessage);
                logger.Info(logMessage);
                currentPageIndex = 1;
                do
                {
                    logMessage = $"正在获取第 {currentPageIndex} 页的项目信息...";
                    Info(logMessage);
                    logger.Info(logMessage);
                    searchResponse = _webClient.SearchInCategory(mapIndexToCategory(lstCategories.CheckedIndices[i]), sortMethod, currentPageIndex);
                    _dbClient.BeginTransaction();
                    for (currentProjectIndexInPage = 0; currentProjectIndexInPage < searchResponse.Projects.Length; ++currentProjectIndexInPage)
                    {
                        logMessage = $"正在统计项目 {currentProjectIndexOfAll + 1}/{totalNeedToCheckCount}...";
                        Info(logMessage);
                        logger.Info(logMessage);
                        b = false;
                        errorCounter = 0;
                        do
                        {
                            try
                            {
                                project = _webClient.GetProject(searchResponse.Projects[currentProjectIndexInPage].ID);
                                logMessage = $"收到项目 #{currentProjectIndexOfAll + 1} (ID={project.ID}) {project.Name}";
                                Info(logMessage);
                                logger.Info(logMessage);
                                _dbClient.InsertRecord(project);
                                b = true;
                            }
                            catch (WebException ex)
                            {
                                logMessage = $"WEB: {ex.Message}";
                                Info(logMessage);
                                logger.Error(logMessage, ex);
                            }
                            catch (Exception ex)
                            {
                                //Info($"Unknown: {ex.Message}");
                                logMessage = "出现错误，请见日志。";
                                Info(logMessage);
                                logger.Error(ex.Message, ex);
                            }
                            if (_cancellation.IsCancellationRequested)
                            {
                                _dbClient.Commit();
                                logMessage = "工作被中止。";
                                Info(logMessage);
                                logger.Warn(logMessage);
                                FinishWork();
                                return;
                            }
                            ++errorCounter;
                        } while (!b && errorCounter < ERROR_LIMIT);
                        logMessage = $"完成项目 {currentProjectIndexOfAll + 1}/{totalNeedToCheckCount}。";
                        Info(logMessage);
                        logger.Info(logMessage);
                        ++currentProjectIndexOfAll;
                        pbProgress.Value = currentProjectIndexOfAll;
                        percent = Math.Round(currentProjectIndexOfAll / (double)totalNeedToCheckCount * 100, 2);
                        SetProgressTitle(percent);
                        lblProgressText.Text = percent.ToString() + "%";
                    }
                    logMessage = "正在写入...";
                    Info(logMessage);
                    logger.Info(logMessage);
                    _dbClient.Commit();
                    ++currentPageIndex;
                } while (searchResponse.HasMore);
            }

            // All work are done.
            logMessage = "完成。";
            Info(logMessage);
            logger.Info(logMessage);
            FinishWork();
        }

        private void FinishWork()
        {
            SetProgressTitle(-1);
            _dbClient.Close();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lstCategories.Enabled = true;
            cboSortMethod.Enabled = true;
            if (_cancellation != null)
            {
                _cancellation.Dispose();
            }
            _cancellation = null;
            _currentTask = null;
        }

        private void Info(string info)
        {
            var str = $"{DateTime.Now.ToShortTimeString()}> {info}";
            txtInfo.AppendText(str);
            txtInfo.AppendText(Environment.NewLine);
        }

        private static CategoryKind mapIndexToCategory(int index)
        {
            return (CategoryKind)(index + 2);
        }

        private static SortMethod mapIndexToSortMethod(int index)
        {
            return (SortMethod)index;
        }

        private DialogResult MsgBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, Application.ProductName, buttons, icon);
        }

        private void SetProgressTitle(double percent)
        {
            if (percent < 0)
            {
                Text = TITLE;
            }
            else
            {
                Text = $"({percent}%) - {TITLE}";
            }
        }

        private KsFetchDatabaseClient _dbClient = new KsFetchDatabaseClient();
        private KsFetchWebClient _webClient = new KsFetchWebClient();
        private CancellationTokenSource _cancellation = null;
        private Task _currentTask = null;

        private static string TITLE = "Kickstarter Abstract Fetcher";

    }

}
