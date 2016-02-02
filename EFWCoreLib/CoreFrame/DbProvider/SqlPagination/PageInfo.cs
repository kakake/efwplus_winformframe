
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================


namespace EFWCoreLib.CoreFrame.DbProvider.SqlPagination
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        private int _pageSize = 20;
        private int _pageNo = 1;
        private int _totalRecord = 0;
        private string _keyName;
        private int _columnLength = 1;


        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 要取的页面，默认为0页
        /// </summary>
        public int pageNo
        {
            get { return _pageNo; }
            set { _pageNo = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage
        {
            get { return totalRecord % pageSize == 0 ? totalRecord / pageSize : totalRecord / pageSize + 1; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalRecord
        {
            get { return _totalRecord; }
            set { _totalRecord = value; }
        }

        public int startNum
        {
            get { return (pageNo - 1) * pageSize + 1; }
        }

        public int endNum
        {
            get { return startNum + pageSize - 1; }
        }

        public string KeyName
        {
            get { return _keyName; }
            set { _keyName = value; }
        }

        /// <summary>
        /// 列的长度
        /// </summary>
        public int ColumnLength
        {
            get { return _columnLength; }
            set { _columnLength = value; }
        }

        public PageInfo(int _pagesize, int _currpagenum)
        {
            _pageSize = _pagesize;
            _pageNo = _currpagenum;
        }

        public PageInfo(int _pagesize, int _currpagenum, int _columnlength)
        {
            _pageSize = _pagesize;
            _pageNo = _currpagenum;
            _columnLength = _columnlength;
        }
    }
}
