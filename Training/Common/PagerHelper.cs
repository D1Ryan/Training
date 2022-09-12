using System.Text;
using System.Web.Mvc;

namespace SAPI.Common
{
    public class WebPager : Pager
    {
        public WebPager(PageModel pageModel)
            : base(pageModel)
        {
        }

        public sealed override string ToString()
        {
            this.ItemCount(5);
            if (_pagemodel.TotalCount == 0 || _pagemodel.TotalCount <= _pagemodel.PageSize)
                return null;

            StringBuilder html = new StringBuilder();


            if (_showfirst)
            {
                if (_pagemodel.IsFirstPage)
                {
                    //html.Append("<li><a href=\"javascript:void(0);\" page=\"1\" class=\"first-page\">首页</a></li>");
                }
                else
                    html.Append("<li><a href=\"javascript:void(0);\" page=\"1\" class=\"first-page\">首页</a></li>");
            }

            if (_showpagesize)
            {
                html.AppendFormat("<li><input type=\"hidden\" value=\"{0}\" id=\"pageSize\" name=\"size\"/></li>", _pagemodel.PageSize);
            }


            if (_showpre)
            {
                if (_pagemodel.HasPrePage)
                    html.AppendFormat("<li><a href=\"javascript:void(0);\" page=\"{0}\" class=\"prev-page\">上一页</a></li>", _pagemodel.PageNumber - 1);
                else
                {
                    //html.Append("<li><a href=\"javascript:void(0);\">上一页</a></li>");
                }
            }

            //if (_showsummary)
            //{
            //    //html.Append(string.Format("<div class=\"summary\">当前{2}/{1}页&nbsp;共{0}条记录</div>", _pagemodel.TotalCount, _pagemodel.TotalPages, _pagemodel.PageNumber));
            //    //html.Append("&nbsp;");
            //    html.Append(string.Format("<li class=\"summary\">{2}/{1}</li>", _pagemodel.TotalCount, _pagemodel.TotalPages, _pagemodel.PageNumber));
            //}
            if (_showitems)
            {
                int startPageNumber = GetStartPageNumber();
                int endPageNumber = GetEndPageNumber();
                for (int i = startPageNumber; i <= endPageNumber; i++)
                {
                    if (_pagemodel.PageNumber != i)
                        html.AppendFormat("<li><a href=\"javascript:void(0);\" page=\"{0}\" class=\"page\">{0}</a></li>", i);
                    else
                        html.AppendFormat("<li class=\"active\"><a href=\"javascript:void(0);\" class=\"page\" page=\"{0}\">{0}</a></li>", i);
                }
            }

            if (_shownext)
            {
                if (_pagemodel.HasNextPage)
                    html.AppendFormat("<li><a href=\"javascript:void(0);\" page=\"{0}\" class=\"next-page\">下一页</a></li>", _pagemodel.PageNumber + 1);
                else
                {
                    //html.Append("<li><a href=\"javascript:void(0);\">下一页</a></li>");
                }
            }

            if (_showgopage)
            {
                html.AppendFormat("<li><input type=\"hidden\" value=\"{0}\" id=\"pageNumber\" totalPages=\"{1}\" name=\"since\"/></li>", _pagemodel.PageNumber, _pagemodel.TotalPages);
            }


            if (_showlast)
            {
                if (_pagemodel.IsLastPage)
                {
                    //html.Append("<li><a href=\"#\" class=\"last-page\">末页</a></li>");
                }
                else
                    html.AppendFormat("<li><a href=\"javascript:void(0);\" page=\"{0}\" class=\"last-page\">末页</a></li>", _pagemodel.TotalPages);
            }



            return html.ToString();
        }
    }

    /// <summary>
    /// 分页ECMapl扩展
    /// </summary>
    public static class PagerHtmlExtension
    {
        /// <summary>
        /// 后台分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static AdminPager AdminPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new AdminPager(pageModel);
        }

        /// <summary>
        /// 前台分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static WebPager WebPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new WebPager(pageModel);
        }
    }

    /// <summary>
    /// 分页基类
    /// </summary>
    public abstract class Pager
    {
        protected readonly PageModel _pagemodel;//分页对象
        protected bool _showsummary = true;//是否显示汇总
        protected bool _showitems = true;//是否显示页项
        protected int _itemcount = 7;//项数量
        protected bool _showfirst = true;//是否显示首页
        protected bool _showpre = true;//是否显示上一页
        protected bool _shownext = true;//是否显示下一页
        protected bool _showlast = true;//是否显示末页
        protected bool _showpagesize = false;//是否显示每页数
        protected bool _showgopage = true;//是否显示页数输入框

        public Pager(PageModel pageModel)
        {
            _pagemodel = pageModel;
        }

        /// <summary>
        /// 设置是否显示汇总
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowSummary(bool value)
        {
            _showsummary = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示页项
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowItems(bool value)
        {
            _showitems = value;
            return this;
        }
        /// <summary>
        /// 设置项数量
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ItemCount(int count)
        {
            _itemcount = count;
            return this;
        }
        /// <summary>
        /// 设置是否显示首页
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowFirst(bool value)
        {
            _showfirst = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示上一页
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowPre(bool value)
        {
            _showpre = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示下一页
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowNext(bool value)
        {
            _shownext = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示末页
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowLast(bool value)
        {
            _showlast = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示每页数
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowPageSize(bool value)
        {
            _showpagesize = value;
            return this;
        }
        /// <summary>
        /// 设置是否显示页数输入框
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public Pager ShowGoPage(bool value)
        {
            _showgopage = value;
            return this;
        }
        /// <summary>
        /// 获得开始页数
        /// </summary>
        /// <returns></returns>
        protected int GetStartPageNumber()
        {
            int mid = _itemcount / 2;
            if ((_pagemodel.TotalPages < _itemcount) || ((_pagemodel.PageNumber - mid) < 1))
            {
                return 1;
            }
            if ((_pagemodel.PageNumber + mid) > _pagemodel.TotalPages)
            {
                return _pagemodel.TotalPages - _itemcount + 1;
            }
            return _pagemodel.PageNumber - mid;
        }
        /// <summary>
        /// 获得结束页数
        /// </summary>
        /// <returns></returns>
        protected int GetEndPageNumber()
        {
            int mid = _itemcount / 2;
            if ((_itemcount % 2) == 0)
            {
                mid--;
            }
            if ((_pagemodel.TotalPages < _itemcount) || ((_pagemodel.PageNumber + mid) >= _pagemodel.TotalPages))
            {
                return _pagemodel.TotalPages;
            }
            if ((_pagemodel.PageNumber - (_itemcount / 2)) < 1)
            {
                return _itemcount;
            }
            return _pagemodel.PageNumber + mid;
        }
    }


    /// <summary>
    /// 后台分页类
    /// </summary>
    public class AdminPager : Pager
    {
        public AdminPager(PageModel pageModel)
            : base(pageModel)
        {
        }

        public sealed override string ToString()
        {
            if (_pagemodel.TotalCount == 0 || _pagemodel.TotalCount <= _pagemodel.PageSize)
                return null;

            StringBuilder ECMapl = new StringBuilder();

            if (_showsummary)
            {
                ECMapl.Append(string.Format("<div class=\"summary\">当前{2}/{1}页&nbsp;共{0}条记录</div>", _pagemodel.TotalCount, _pagemodel.TotalPages, _pagemodel.PageNumber));
                ECMapl.Append("&nbsp;");
            }

            if (_showpagesize)
            {
                ECMapl.AppendFormat("每页:<input type=\"text\" value=\"{0}\" id=\"pageSize\" name=\"size\" size=\"1\"/>", _pagemodel.PageSize);
            }

            if (_showfirst)
            {
                if (_pagemodel.IsFirstPage)
                    ECMapl.Append("<a href=\"#\">首页</a>");
                else
                    ECMapl.Append("<a href=\"#\" page=\"1\" class=\"bt\">首页</a>");
            }

            if (_showpre)
            {
                if (_pagemodel.HasPrePage)
                    ECMapl.AppendFormat("<a href=\"#\" page=\"{0}\" class=\"bt\">上一页</a>", _pagemodel.PageNumber - 1);
                else
                    ECMapl.Append("<a href=\"#\">上一页</a>");
            }

            if (_showitems)
            {
                int startPageNumber = GetStartPageNumber();
                int endPageNumber = GetEndPageNumber();
                for (int i = startPageNumber; i <= endPageNumber; i++)
                {
                    if (_pagemodel.PageNumber != i)
                        ECMapl.AppendFormat("<a href=\"#\" page=\"{0}\" class=\"bt\">{0}</a>", i);
                    else
                        ECMapl.AppendFormat("<a href=\"\" class=\"hot\">{0}</a>", i);
                }
            }

            if (_shownext)
            {
                if (_pagemodel.HasNextPage)
                    ECMapl.AppendFormat("<a href=\"#\" page=\"{0}\" class=\"bt\">下一页</a>", _pagemodel.PageNumber + 1);
                else
                    ECMapl.Append("<a href=\"#\">下一页</a>");
            }

            if (_showlast)
            {
                if (_pagemodel.IsLastPage)
                    ECMapl.Append("<a href=\"#\">末页</a>");
                else
                    ECMapl.AppendFormat("<a href=\"#\" page=\"{0}\" class=\"bt\">末页</a>", _pagemodel.TotalPages);
            }

            if (_showgopage)
            {
                ECMapl.AppendFormat("跳转到:<input type=\"text\" value=\"{0}\" id=\"pageNumber\" totalPages=\"{1}\" name=\"since\" size=\"1\"/>页", _pagemodel.PageNumber, _pagemodel.TotalPages);
            }

            return ECMapl.ToString();
        }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class PageModel
    {
        private int _pageindex;//当前页索引
        private int _pagenumber;//当前页数
        private int _prepagenumber;//上一页数
        private int _nextpagenumber;//下一页数
        private int _pagesize;//每页数
        private int _totalcount;//总项数
        private int _totalpages;//总页数
        private bool _hasprepage;//是否有上一页
        private bool _hasnextpage;//是否有下一页
        private bool _isfirstpage;//是否是第一页
        private bool _islastpage;//是否是最后一页

        public PageModel(int pageSize, int pageNumber, int totalCount)
        {
            if (pageSize > 0)
                _pagesize = pageSize;
            else
                _pagesize = 1;

            if (pageNumber > 0)
                _pagenumber = pageNumber;
            else
                _pagenumber = 1;

            if (totalCount > 0)
                _totalcount = totalCount;
            else
                _totalcount = 0;

            _pageindex = _pagenumber - 1;

            _totalpages = _totalcount / _pagesize;
            if (_totalcount % _pagesize > 0)
                _totalpages++;

            _hasprepage = _pagenumber > 1;
            _hasnextpage = _pagenumber < _totalpages;

            _isfirstpage = _pagenumber == 1;
            _islastpage = _pagenumber == _totalpages;

            _prepagenumber = _pagenumber < 2 ? 1 : _pagenumber - 1;
            _nextpagenumber = _pagenumber < _totalpages ? _pagenumber + 1 : _totalpages;
        }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return _pageindex; }
            set { _pageindex = value; }
        }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageNumber
        {
            get { return _pagenumber; }
            set { _pagenumber = value; }
        }
        /// <summary>
        /// 上一页数
        /// </summary>
        public int PrePageNumber
        {
            get { return _prepagenumber; }
            set { _prepagenumber = value; }
        }
        /// <summary>
        /// 下一页数
        /// </summary>
        public int NextPageNumber
        {
            get { return _nextpagenumber; }
            set { _nextpagenumber = value; }
        }
        /// <summary>
        /// 每页数
        /// </summary>
        public int PageSize
        {
            get { return _pagesize; }
            set { _pagesize = value; }
        }
        /// <summary>
        /// 总项数
        /// </summary>
        public int TotalCount
        {
            get { return _totalcount; }
            set { _totalcount = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return _totalpages; }
            set { _totalpages = value; }
        }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrePage
        {
            get { return _hasprepage; }
            set { _hasprepage = value; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return _hasnextpage; }
            set { _hasnextpage = value; }
        }
        /// <summary>
        /// 是否是第一页
        /// </summary>
        public bool IsFirstPage
        {
            get { return _isfirstpage; }
            set { _isfirstpage = value; }
        }
        /// <summary>
        /// 是否是最后一页
        /// </summary>
        public bool IsLastPage
        {
            get { return _islastpage; }
            set { _islastpage = value; }
        }
    }    
}