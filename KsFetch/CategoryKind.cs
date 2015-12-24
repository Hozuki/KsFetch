using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch
{

    public enum CategoryKind
    {

        [AssociatedText("recommended")]
        Recommended = -2,
        Starred = -1,
        Everything = 1,

        [AssociatedText("art")]
        [ChineseDescription("艺术")]
        Art,
        [AssociatedText("comics")]
        [ChineseDescription("漫画")]
        Comics,
        [AssociatedText("crafts")]
        [ChineseDescription("手工艺品")]
        Crafts,
        [AssociatedText("dance")]
        [ChineseDescription("舞蹈")]
        Dance,
        [AssociatedText("design")]
        [ChineseDescription("设计")]
        Design,
        [AssociatedText("fashion")]
        [ChineseDescription("时尚")]
        Fashion,
        [AssociatedText("film%20&%20video")]
        [ChineseDescription("影视")]
        FilmAndVideo,
        [AssociatedText("food")]
        [ChineseDescription("美食")]
        Food,
        [AssociatedText("games")]
        [ChineseDescription("游戏")]
        Games,
        [AssociatedText("journalism")]
        [ChineseDescription("新闻")]
        Journalism,
        [AssociatedText("music")]
        [ChineseDescription("音乐")]
        Music,
        [AssociatedText("photography")]
        [ChineseDescription("摄影")]
        Photography,
        [AssociatedText("publishing")]
        [ChineseDescription("出版物")]
        Publishing,
        [AssociatedText("technology")]
        [ChineseDescription("技术")]
        Technology,
        [AssociatedText("theater")]
        [ChineseDescription("戏剧")]
        Theater

    }

}
