using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace EFWCoreLib.WebFrame.HttpHandler.Controller
{
    /// <summary>
    /// 对象转与JqueryEasyUI匹配的Json格式
    /// </summary>
    public interface IToJqueryEasyUIJson
    {

        //string TxtJson { get; set; }
        string FilterJson(string json);//过滤json字符串中所有特殊字符

        string ToJson(object model);
        string ToJson(System.Data.DataTable dt);//转json字符串，combobox控件用此方法
        string ToJson(Hashtable hash);

        string ToGridJson(string tojson, int totalCount);
        string ToGridJson(string rowsjson, string footjson, int totalCount);

        string ToGridJson(System.Data.DataTable dt);
        string ToGridJson(System.Data.DataTable dt, int totalCount);
        string ToGridJson(System.Data.DataTable dt, int totalCount, System.Collections.Hashtable[] footers);


        string ToGridJson(System.Collections.IList list);
        string ToGridJson(System.Collections.IList list, int totalCount);
        string ToGridJson(System.Collections.IList list, int totalCount, System.Collections.Hashtable[] footers);

        string ToFloorJson(List<floorclass> floor);
        string ToFloorJson(List<floorclass> floor, int totalCount);

        string ToTreeJson(List<treeNode> list);

        string ToTreeGridJson(List<treeNodeGrid> list);
        string ToTreeGridJson(List<treeNodeGrid> list, System.Collections.Hashtable[] footers);
        string ToTreeGridJson(DataTable dt, string IdName, string _parentIdName);
        string ToTreeGridJson(DataTable dt, string IdName, string _parentIdName, System.Collections.Hashtable[] footers);
        string ToTreeGridJson(string rowsjson, string footjson, int totalCount);

        T GetModel<T>();//从form表单提交的数据转为实体对象
        T GetModel<T>(T model);//从form表单提交的数据赋值给model


        DataTable ToDataTable(string json);
        List<T> ToList<T>(string json);

        //string ReturnSuccess();
        //string ReturnSuccess(string info);
        //string ReturnSuccess(string info, string data);
        //string ReturnError(string errmsg);

        //string ToView();//回退
        //string ToView(string info);//提示后再回退
        //string ToView(string info, string Url);//提示后调整到执行页面



    }

    public class treeNode
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _text;

        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        private bool _check = false;
        public bool check
        {
            get { return _check; }
            set { _check = value; }
        }

        private string _state = "open";
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _iconCls = "";
        public string iconCls
        {
            get { return _iconCls; }
            set { _iconCls = value; }
        }

        private List<treeNode> _children;

        public List<treeNode> children
        {
            get { return _children; }
            set { _children = value; }
        }

        private Dictionary<string, Object> _attributes;
        public Dictionary<string, Object> attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public treeNode(int id, string text)
        {
            _id = id;
            _text = text;
        }
    }

    public class treeNodeGrid
    {
        private string _state = "";
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        public string iconCls
        {
            get;
            set;
        }

        private bool _check = false;
        public bool check
        {
            get { return _check; }
            set { _check = value; }
        }

        public int id { get; set; }

        public int _parentId { get; set; }

        public object model { get; set; }
    }

    public class floorclass
    {
        private int _floorid;

        public int floorid
        {
            get { return _floorid; }
            set { _floorid = value; }
        }
        private string _floortext;

        public string floortext
        {
            get { return _floortext; }
            set { _floortext = value; }
        }
        private DataTable _room;

        public DataTable room
        {
            get { return _room; }
            set { _room = value; }
        }
    }
}
