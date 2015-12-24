using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch
{

    public enum SortMethod
    {

        [AssociatedText("magic")]
        [ChineseDescription("魔术排序(随机?)")]
        Magic,
        [AssociatedText("popularity")]
        [ChineseDescription("按人气(从高到低)")]
        Popularity,
        [AssociatedText("newest")]
        [ChineseDescription("按时间(从后往前)")]
        Newest,
        [AssociatedText("end_date")]
        [ChineseDescription("按结束时间(从后往前)")]
        EndDate,
        [AssociatedText("most_funded")]
        [ChineseDescription("按收到投资(从多到少)")]
        MostFunded

    }

}
