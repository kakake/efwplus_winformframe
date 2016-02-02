using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace EfwControls.CustomControl
{

    public enum pageSizeType
    {
        Size10 = 0, Size20 = 1, Size50 = 2, Size100 = 3, Size200 = 4
    }

    public delegate void PagerEventHandler(object sender, int pageNo, int pageSize);

    /// <summary>
    /// 分页控件呈现
    /// 两种分页模式：
    /// 1.传入全部数据，控件本身对数据进行分页
    /// 2.只传入一页的数据和总记录数，传出PageNo和PageSize
    /// </summary>
    public partial class Pager : UserControl
    {
        public Pager()
        {
            InitializeComponent();
            
        }


        /// <summary>
        /// 每页显示记录数
        /// </summary>
        private pageSizeType _pageSizeType = pageSizeType.Size10;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public pageSizeType PageSizeType
        {
            get { return _pageSizeType; }
            set
            {
                _pageSizeType = value;
                this.comboBoxEx1.SelectedIndex = (int)PageSizeType;
            }
        }

        private int _pageSize = 10;
        private int _pageNo = 1;
        private int _totalRecord = 100;


        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                this.labelX1.Text = String.Format("第{0}页|共{1}页|{2}条", pageNo, totalPage, totalRecord);
                this.slider1.Maximum = totalPage * pageSize;
                this.slider1.Minimum = endNum;
                this.slider1.Step = pageSize;
                this.slider1.ValueChanged -= new System.EventHandler(this.slider1_ValueChanged);
                this.slider1.Value = endNum;
                this.slider1.ValueChanged += new System.EventHandler(this.slider1_ValueChanged);

                if (PageNoChanged != null)
                    if (isPage == false)
                        PageNoChanged(this, _pageNo, pageSize);

                PagerSource();
            }
        }

        /// <summary>
        /// 要取的页面，默认为1页
        /// </summary>
        public int pageNo
        {
            get { return _pageNo; }
            set
            {
                _pageNo = value;
                if (_pageNo == 0)
                    _pageNo = 1;
                if (_pageNo > totalPage)
                    _pageNo = totalPage;
                this.labelX1.Text = String.Format("第{0}页|共{1}页|{2}条", pageNo, totalPage, totalRecord);
                this.slider1.ValueChanged -= new System.EventHandler(this.slider1_ValueChanged);
                this.slider1.Value = _pageNo * pageSize;
                this.slider1.ValueChanged += new System.EventHandler(this.slider1_ValueChanged);
                if (PageNoChanged != null)
                    if (isPage == false)
                        PageNoChanged(this, _pageNo, pageSize);
                PagerSource();
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalRecord
        {
            get { return _totalRecord; }
            set
            {
                _totalRecord = value;
                this.labelX1.Text = String.Format("第{0}页|共{1}页|{2}条", pageNo, totalPage, totalRecord);
                this.slider1.Maximum = totalPage * pageSize;

                PagerSource();
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage
        {
            get { return totalRecord % pageSize == 0 ? totalRecord / pageSize : totalRecord / pageSize + 1; }
        }

        public int startNum
        {
            get { return (pageNo - 1) * pageSize + 1; }
        }

        public int endNum
        {
            get
            {
                return (startNum + pageSize - 1) > totalRecord ? totalRecord : (startNum + pageSize - 1);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.comboBoxEx1.SelectedIndex = (int)PageSizeType;
            //if (IsPage == false)
                pageNo = 1;
        }

        private void slider1_ValueChanged(object sender, EventArgs e)
        {
            int value = this.slider1.Value;
            //pageNo = value % pageSize > 0 ? value / pageSize + 1 : value / pageSize;
            pageNo = value / pageSize;
            //PagerSource();
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _pageNo = 1;
            if (comboBoxEx1.SelectedItem != null)
                pageSize = Convert.ToInt32(comboBoxEx1.SelectedItem.ToString());


            //PagerSource();
        }


        private DataGridView _grid;
        [Description("绑定数据控件")]
        [DefaultValue("")]
        public DataGridView BindDataControl
        {
            get { return _grid; }
            set { _grid = value; }
        }

        private DataTable _source=null;
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        [AttributeProvider(typeof(IListSource))]
        public DataTable DataSource
        {
            get { return _source; }
            set
            {
                _source = value;
                if (_source != null)
                {
                    if (IsPage == true)
                    {
                        _pageNo = 1;
                        totalRecord = _source.Rows.Count;
                    }
                }
                else
                    totalRecord = 0;
            }
        }

        private void PagerSource()
        {
            if (_source != null)
            {
                this._grid.AutoGenerateColumns = false;
                //_grid.DataSource = null;
                if (isPage == false)
                    _grid.DataSource = _source;
                else
                {
                    _grid.DataSource = GetPagerSource(_source);
                }
            }
            else
            {
                if (_grid != null)
                    _grid.DataSource = null;
            }
        }

        private object GetPagerSource(DataTable dt)
        {

            DataTable _dt = dt.Clone();
            if (dt.Rows.Count > 0)
                for (int i = startNum - 1; i < (dt.Rows.Count - startNum + 1 < pageSize ? dt.Rows.Count : endNum); i++)
                {
                    _dt.Rows.Add(dt.Rows[i].ItemArray);
                }

            return _dt;
        }
        /// <summary>
        /// 是否内部分页
        /// </summary>
        private bool isPage = true;
        [Description("设置是否内部分页，IsPage=false时结合PagerEventHandler事件使用")]
        public bool IsPage
        {
            get { return isPage; }
            set { isPage = value; }
        }
        [Description("翻页的事件，结合IsPage=false属性使用")]
        public event PagerEventHandler PageNoChanged;
    }
   
}
