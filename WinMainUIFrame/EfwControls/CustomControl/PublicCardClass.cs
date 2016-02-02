using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace EfwControls.CustomControl
{

    #region TextBoxCard
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum MatchModes
    {
        /// <summary>
        /// 任意字符
        /// </summary>
        ByAnyString = 0,
        /// <summary>
        /// 按首字符
        /// </summary>
        ByFirstChar = 1
    }

    public delegate void AfterSelectedRowHandler(object sender, object SelectedValue);

    /// <summary>
    /// 翻页事件
    /// </summary>
    /// <param name="sender">对象</param>
    /// <param name="pageNo">当前页</param>
    /// <param name="pageSize">页大小</param>
    public delegate void PagerTextEventHandler(object sender, int pageNo, int pageSize, string fiterChar);

    #endregion

    #region GridBoxCard
    /// <summary>
    /// DataGridViewEx中的选项卡选中行后的事件委托
    /// </summary>
    /// <param name="SelectedValue"></param>
    /// <param name="stop">停止跳转</param>
    /// <param name="customNextColumnIndex">跳转到指定列</param>
    public delegate void OnSelectCardRowSelectedHandle(object SelectedValue, ref bool stop, ref int customNextColumnIndex);
    /// <summary>
    /// 翻页事件
    /// </summary>
    /// <param name="sender">对象</param>
    /// <param name="index">ShowCard数据源索引</param>
    /// <param name="pageNo">当前页</param>
    /// <param name="pageSize">页大小</param>
    public delegate void PagerGridEventHandler(object sender, int index, int pageNo, int pageSize, string fiterChar);

    /// <summary>
    /// 自动新增一行时给dataRow赋值
    /// </summary>
    /// <param name="dataRow">新增一行赋值</param>
    public delegate void UserAddGirdRowHandler(DataRow dataRow);
    /// <summary>
    ///  自动新增一行自定义规则
    /// </summary>
    /// <returns></returns>
    public delegate bool UserAddGridRowCustom();

    /// <summary>
    /// 单元格回车事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="colIndex">当前列号</param>
    /// <param name="rowIndex">当前行号</param>
    /// <param name="jumpStop">停止跳转</param>
    public delegate void OnDataGridViewCellPressEnterKeyHandle(object sender, int colIndex, int rowIndex, ref bool jumpStop);


    /// <summary>
    /// 网格选项卡类
    /// </summary>
    public class DataGridViewSelectionCard
    {
        private DataGridViewTextBoxColumn[] columns;
        private System.Data.DataTable dataSource;
        private System.Drawing.Size cardSize = new Size(350, 276);
        private MatchModes selectCardFilterType;
        private int bindColumnIndex;
        private DataRow[] filterResult;
        public DataGridViewSelectionCard()
        {

        }

        private string _CardColumn;
        [Description("选项卡列信息,如：Code|编码|80,Name|名称|120")]
        public string CardColumn
        {
            get { return _CardColumn; }
            set { _CardColumn = value; }
        }

        /// <summary>
        /// 选择卡列信息
        /// </summary>
        public DataGridViewTextBoxColumn[] ShowCardColumns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }
        /// <summary>
        /// 选择卡数据源
        /// </summary>
        public System.Data.DataTable DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
            }
        }
        /// <summary>
        /// 选择卡大小
        /// </summary>
        public System.Drawing.Size CardSize
        {
            get
            {
                return cardSize;
            }
            set
            {
                cardSize = value;
            }
        }

        /// <summary>
        /// 设置查询字段,通过“,”分割
        /// </summary>
        [Description("设置查询字段,通过“,”分割")]
        public string QueryFieldsString
        {
            get
            {
                string strValue = "";
                if (queryFields != null)
                {
                    for (int i = 0; i < queryFields.Length; i++)
                    {
                        if (strValue == "")
                            strValue = queryFields[i];
                        else
                            strValue += "," + queryFields[i];
                    }
                }
                return strValue;
            }
            set
            {
                queryFields = value.Split(',');
            }
        }

        /// <summary>
        /// 获取或设置查询字段
        /// </summary>
        private string[] queryFields;
        [Description("获取或设置查询字段")]
        public string[] QueryFields
        {
            get
            {
                return queryFields;
            }
            set
            {
                queryFields = value;
            }
        }

        /// <summary>
        /// 选择卡记录过滤方式
        /// </summary>
        public MatchModes SelectCardFilterType
        {
            get
            {
                return selectCardFilterType;
            }
            set
            {
                selectCardFilterType = value;
            }
        }

        /// <summary>
        /// 获取或设置选项卡要绑定到的列的索引
        /// </summary>
        public int BindColumnIndex
        {
            get
            {
                return bindColumnIndex;
            }
            set
            {
                bindColumnIndex = value;
            }
        }

        public DataRow[] FilterResult
        {
            get
            {
                return filterResult;
            }
            set
            {
                filterResult = value;
            }
        }

        private bool _isPage=true;
        [Description("设置是否内部分页，IsPage=false时结合PagerEventHandler事件使用")]
        public bool IsPage
        {
            get { return _isPage; }
            set { _isPage = value; }
        }

        private int _pageTotalRecord;
        /// <summary>
        /// 总记录数 IsPage=false时才使用
        /// </summary>
        public int PageTotalRecord
        {
            get { return _pageTotalRecord; }
            set { _pageTotalRecord = value; }
        }

        private string _memo;
        [Description("选项卡数据源说明")]
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

    }

    /// <summary>
    /// 网格数字键盘
    /// </summary>
    public class DataGridViewSelectionNumericKeyBoard
    {

        private int bindColumnIndex;

        public DataGridViewSelectionNumericKeyBoard()
        {

        }


        /// <summary>
        /// 获取或设置选项卡要绑定到的列的索引
        /// </summary>
        public int BindColumnIndex
        {
            get
            {
                return bindColumnIndex;
            }
            set
            {
                bindColumnIndex = value;
            }
        }


        private string _memo;
        [Description("选项卡数据源说明")]
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

    }
    #endregion
}
