using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using Microsoft.EntityFrameworkCore;

using SAPI.Models;

namespace SAPI.DAL
{
    public class Person
    {
        public string Name;
        public int Age;
        public float Weight;
        public string Hobby;
    }

    public class EFDemo
    {
        private List<Person> m_DataSourc;

        ScheduleEntities sdb = new ScheduleEntities();

        //构造函数中初始化演示数据源
        public EFDemo()
        {
            m_DataSourc = new List<Person>
            {
                new Person{Name="熊大", Age=31,Weight=125,Hobby="吃饭"},
               new Person{Name="熊二", Age=30,Weight=120,Hobby="睡觉"},
                new Person{Name="光头强", Age=28,Weight=70,Hobby="砍树"},
                  new Person{Name="喜羊羊", Age=12,Weight=20,Hobby="自拍"},
                  new Person{Name="美羊羊", Age=13,Weight=21,Hobby="自拍"},
                  new Person{Name="懒羊羊", Age=14,Weight=22,Hobby="睡觉"},
                   new Person{Name="灰太狼", Age=30,Weight=60,Hobby="抓羊"}
            };
        }

        public void Search(string name = null, int? age = null, float? weight = null, string hobby = null)
        {
            //为了模拟EF查询，转换为IEnumerable,在EF中此处为数据库上下文的表对象
            var result = (IEnumerable<Person>)m_DataSourc;

            /*下列代码不会立即执行查询，而是生成查询计划
             * 若参数不存在则不添加查询条件，从而可以无限制的添加查询条件
             */
            if (!string.IsNullOrEmpty(name)) { result = result.Where(d => d.Name.Contains(name)); }
            if (age.HasValue) { result = result.Where(d => d.Age >= age); }
            if (weight.HasValue) { result = result.Where(d => d.Weight >= weight); }
            if (!string.IsNullOrEmpty(hobby)) { result = result.Where(d => d.Hobby.Contains(hobby)); }

            //此时执行查询
            var final = result.ToList();
            Console.WriteLine("");
            Console.WriteLine("*******开始结束*******");
            foreach (var i in final)
            {
                Console.WriteLine("姓名：{0}  年龄：{1}  体重 ：{2}  爱好：{3}", i.Name, i.Age, i.Weight, i.Hobby);
            }
            Console.WriteLine("*******查询结束*******");
            Console.WriteLine("");
        }


        //EFDemo demo = new EFDemo();
        ////无参数查询
        //demo.Search();
        //    //一个参数查询
        //    demo.Search("羊");
        //    //两个参数查询
        //    demo.Search("羊", 13);
        //    //三个参数查询
        //    demo.Search("羊", 13,21);
        //    //四个参数查询
        //    demo.Search("羊", 13, 21,"睡");
        //    Console.Read();


        //https://blog.csdn.net/echoshinian100/article/details/45244989


        ///// <summary>
        ///// 分页查询 + 条件查询 + 排序
        ///// </summary>
        ///// <typeparam name="Tkey">泛型</typeparam>
        ///// <param name="pageSize">每页大小</param>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="total">总数量</param>
        ///// <param name="whereLambda">查询条件</param>
        ///// <param name="orderbyLambda">排序条件</param>
        ///// <param name="isAsc">是否升序</param>
        ///// <returns>IQueryable 泛型集合</returns>
        //public IQueryable<Schedule> QuerySchedule<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<Schedule, bool>> whereLambda, Func<Schedule, Tkey> orderbyLambda, bool isAsc)
        //{

        //    total = sdb.Set<Schedule>().Where(whereLambda.Compile()).Count();
        //if (isAsc)
        //{
        //    var temp = sdb.Set<Schedule>().Where(whereLambda.Compile())
        //                 .OrderBy<Schedule.StartDate, Tkey>(orderbyLambda)
        //                 .Skip(pageSize * (pageIndex - 1))
        //                 .Take(pageSize);
        //    return temp.AsQueryable();
        //}
        //else
        //{
        //    var temp = sdb.Set<Schedule>().Where(whereLambda.Compile())
        //               .OrderByDescending<StartDate, Tkey>(orderbyLambda)
        //               .Skip(pageSize * (pageIndex - 1))
        //               .Take(pageSize);
        //    return temp.AsQueryable();
        //}
        //}


        public void Test()
        {
            //var eps = DynamicLinqExpressions.True<Schedule>();// 多条件查询集合
            //DateTime date = DateTime.Parse(dateTimeInput1.Value.ToString("yyyy-MM-dd "));
            //if (date.ToString().Split('/')[0] != "0001")
            //{
            //    //关于时间 由于需求是查询特定某一天的数据。数据库中 时间字段包含 00:00:00
            //    //于是转换了下
            //    eps = eps.And(u => u.time.Value.ToString("yyyy-MM-dd ") == date.ToString("yyyy-MM-dd "));
            //}
            //string username = txtuserName.Text;
            //if (username != "")
            //{
            //    eps = eps.And(u => u.username == username);
            //}
            //string opt = comboBox1.SelectedItem.ToString();
            //if (opt != "请选择")
            //{
            //    eps = eps.And(u => u.opt == opt);
            //}
            //int pagesize = int.Parse(cmd_pagecount.SelectedItem.ToString());//每页显示多少条数据
            //int count = 0;//返回的页数
            ////调用分页
            //var log = dPShelper.LoadPage_log(pagesize, CurrentPage, out count, eps, u => u.id, true);
            //txtpage.Text = count.ToString();//有多少条数据
            //txtpagesizeCount.Text = string.Format("{0}", (count + 1) / pagesize + 1); ;//得到总页数
            //txtpagesize.MaxValue = int.Parse(txtpagesizeCount.Text);

            //DGV1.AutoGenerateColumns = false;
            //DGV1.DataSource = log.ToList();
            //绑定数据的时候要 toLisst() 不然会报异常                                           
        }

        ////串表
        ///// <summary>
        ///// card_A 是自己定义的实体类
        ///// </summary>
        ///// <returns></returns>
        ////已发卡查询
        //public IQueryable<Schedule> LoadPage_dpcinfo<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<dpcinfo, bool>> whereLambda, Func<dpcinfo, Tkey> orderbyLambda, bool isAsc)
        //{
        //total = sdb.Set<Schedule>().Where(whereLambda).Count();
        //if (isAsc)
        //{
        //    var temp = sdb.Set<Schedule>()
        //                    .Where(whereLambda)
        //                    .OrderBy<dpcinfo, Tkey>(orderbyLambda)
        //                    .Skip(pageSize * (pageIndex - 1))
        //                    .Take(pageSize)
        //                    .GroupJoin(MyBaseDbContext.cardda_mxz, d => d.dpcid, c => c.dpcid, (d, c) => new Log.card_A { dpcid = d.dpcid, addr = d.addr, count_add = c.Count() });
        //    return temp.AsQueryable();
        //}
        //else
        //{
        //    var temp = sdb.Set<Schedule>()
        //                   .OrderByDescending<dpcinfo, Tkey>(orderbyLambda)
        //                   .Skip(pageSize * (pageIndex - 1))
        //                   .Take(pageSize)
        //                    .GroupJoin(sdb.Schedule, d => d.dpcid, c => c.dpcid, (d, c) => new Log.card_A { dpcid = d.dpcid, addr = d.addr, count_add = c.Count() });
        //    return temp.AsQueryable();
        //}

        //}
        /*
         查询优化
1、当用户查询 刷新时，从当前页开始刷新，不会跳转到第一页
2、当显示行数增加，触发查询时，如当前页*显示页数超过数据总量时，自动显示最后一页
反之正常显示当前页数
         */
        public void GOselect_card()
        {
            //int pagesize = int.Parse(txtpageNow_card.Text);//当前页数
            //int pagesizeCount = int.Parse(txtpagesizeCount_card.Text);//总页数
            //int pagecount = int.Parse(cmd_pagecount_card.SelectedItem.ToString());//显示多少条
            //int count_user = int.Parse(txtpage_card.Text);//总条数
            //if (pagesize > 0 || pagesize <= pagesizeCount)
            //{
            //    if ((pagesize * pagecount) > count_user && count_user != 0)
            //    {
            //        txtpageNow_card.Value = int.Parse(txtpagesizeCount_card.Text);//最大页数
            //        pagesizeCount = (count_user - 1) / pagecount + 1;//得到总页数
            //        selectcard(pagesizeCount);
            //    }
            //    else
            //    {
            //        selectcard(pagesize);
            //    }
            //}
        }
    }

    public static class DynamicLinqExpressions
    {
        //多条件查询帮助类
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        //注意this
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

}