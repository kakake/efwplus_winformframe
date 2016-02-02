using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseMenu", EntityType = EntityType.Table, IsGB = true)]
    public class BaseMenu:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _menuid;
        /// <summary>
        /// 编号
        /// </summary>
        [Column(FieldName = "MenuId", DataKey = true,  Match = "", IsInsert = false)]
        public int MenuId
        {
            get { return  _menuid; }
            set {  _menuid = value; }
        }

        private int  _sortid;
        /// <summary>
        /// 排序号
        /// </summary>
        [Column(FieldName = "SortId", DataKey = false,  Match = "", IsInsert = true)]
        public int SortId
        {
            get { return  _sortid; }
            set {  _sortid = value; }
        }

        private string  _name;
        /// <summary>
        /// 名称
        /// </summary>
        [Column(FieldName = "Name", DataKey = false,  Match = "", IsInsert = true)]
        public string Name
        {
            get { return  _name; }
            set {  _name = value; }
        }

        private string  _dllname;
        /// <summary>
        /// DLL名称
        /// </summary>
        [Column(FieldName = "DllName", DataKey = false,  Match = "", IsInsert = true)]
        public string DllName
        {
            get { return  _dllname; }
            set {  _dllname = value; }
        }

        private string  _funname;
        /// <summary>
        /// 引出函数名称
        /// </summary>
        [Column(FieldName = "FunName", DataKey = false,  Match = "", IsInsert = true)]
        public string FunName
        {
            get { return  _funname; }
            set {  _funname = value; }
        }

        private string  _funtag;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "FunWcfName", DataKey = false, Match = "", IsInsert = true)]
        public string FunWcfName
        {
            get { return  _funtag; }
            set {  _funtag = value; }
        }

        private int  _moduleid;
        /// <summary>
        /// 所属模块编号
        /// </summary>
        [Column(FieldName = "ModuleId", DataKey = false,  Match = "", IsInsert = true)]
        public int ModuleId
        {
            get { return  _moduleid; }
            set {  _moduleid = value; }
        }

        private int  _pmenuid;
        /// <summary>
        /// 父菜单编号
        /// </summary>
        [Column(FieldName = "PMenuId", DataKey = false,  Match = "", IsInsert = true)]
        public int PMenuId
        {
            get { return  _pmenuid; }
            set {  _pmenuid = value; }
        }

        private int  _menutoolbar;
        /// <summary>
        /// 是否显示在toolbar
        /// </summary>
        [Column(FieldName = "MenuToolBar", DataKey = false,  Match = "", IsInsert = true)]
        public int MenuToolBar
        {
            get { return  _menutoolbar; }
            set {  _menutoolbar = value; }
        }

        private int  _menulookbar;
        /// <summary>
        /// 是否显示在outlookbar
        /// </summary>
        [Column(FieldName = "MenuLookBar", DataKey = false,  Match = "", IsInsert = true)]
        public int MenuLookBar
        {
            get { return  _menulookbar; }
            set {  _menulookbar = value; }
        }

        private string  _memo;
        /// <summary>
        /// 备注
        /// </summary>
        [Column(FieldName = "Memo", DataKey = false,  Match = "", IsInsert = true)]
        public string Memo
        {
            get { return  _memo; }
            set {  _memo = value; }
        }

        private string  _image;
        /// <summary>
        /// 菜单图片
        /// </summary>
        [Column(FieldName = "Image", DataKey = false,  Match = "", IsInsert = true)]
        public string Image
        {
            get { return  _image; }
            set {  _image = value; }
        }

        private string  _urlid;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "UrlId", DataKey = false,  Match = "", IsInsert = true)]
        public string UrlId
        {
            get { return  _urlid; }
            set {  _urlid = value; }
        }

        private string  _urlname;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "UrlName", DataKey = false,  Match = "", IsInsert = true)]
        public string UrlName
        {
            get { return  _urlname; }
            set {  _urlname = value; }
        }

        private string  _bindsql;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "BindSQL", DataKey = false,  Match = "", IsInsert = true)]
        public string BindSQL
        {
            get { return  _bindsql; }
            set {  _bindsql = value; }
        }

    }
}
