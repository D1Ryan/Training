using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace SAPI.Common
{
    public class EFHelper
    {
        public static Expression<Func<T, bool>> GetConditionExpression<T>(string[] options, string fieldName)
        {
            ParameterExpression left = Expression.Parameter(typeof(T), "c");//c=>
            Expression expression = Expression.Constant(false);
            foreach (var optionName in options)
            {
                Expression right = Expression.Call
                       (
                          Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName
                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),// 反射使用.Contains()方法                         
                          Expression.Constant(optionName)           // .Contains(optionName)
                       );
                expression = Expression.Or(right, expression);//c.DataSourceName.contain("") || c.DataSourceName.contain("") 
            }
            Expression<Func<T, bool>> finalExpression
                = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
            return finalExpression;
        }


        /*

         我想用逆推的方式说明下这段代码，其实我们查询的目的要实现这样的效果 ， someList.where(c=>c.Name.contains("someName")||c.Name.Contains("someName")||...)
        1. 首先我们要确定返回什么样的表达式，根据经验.where后面是需要一个Expression<Func<T, bool>> 这样的一个表达式，所以方法的返回类型已经能确定下来了
        2. 接下来的任务是拼接类似于c=>c.Name.Contains("") 这样的表达式，按照自左向右的原则，左侧表达式参数c很好理解 就是T,那么这个表达式的参数也就搞定了，
        可以用Expression.Parameter方法来实现，该方法目的是将类型反射并且映射给表达式中的匿名变量 “c” （也可以理解成将参数常量封装成表达式）
        3. 接着是表达式右侧的拼接
         再次仔细看下这段代码
        Expression right = Expression.Call 

         (                          

             Expression.Property(left, typeof(T).GetProperty(fieldName)),  //c.DataSourceName     首先是反射获取c的一个属性 
             typeof(string).GetMethod("Contains",new Type[] { typeof(string) }),// 声明一个string.Contains的方法     c.DataSourceName.Contains()                反射使用.Contains()方法  
             Expression.Constant(optionName)           //  c.DataSourceName.Contains(optionName)               封装常量       

         );

          为什么要使用Expression.Call ？
        （因为c.Name.contains 属于string.contains()这个方法所以我们必须将该方法封装成表达式，Expression.Call的功能就是将方法封装成表达式）
         这时候大家会问contains什么呢？ 当然常量option虽然是string类型，但是仍需封装成表达式,Expression.Constant(optionName) 起到了封装常量的作用
         于是c=>c.属性.Contains(常量) 这个表达式搞定，可是还是有问题：怎么加上“||” ，聪明的你已经有了答案，Expression.Or()

        4 最后一步当然非常关键，就像产品需要通过流水线进行包装组合，表达式也不例外：
        Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });
        对于整个表达式来说，左侧是参数表达式（ParameterExpression），Expression.Lambda就是=>符号，就右侧表达式和参数表达式通过lambda符号进行组合，搞定
        这样的话，你只需传入一个字符串数组就能在Linq中实现类似于sql中select in 的效果了，
        很多朋友肯定会问，既然能够用自定义表达式搞定，
        那么可不可以将表达式的思路用于拼接sql？
        答案是肯定的。
         */



        /*
         不要用Func<TSource, bool>，用Expression<Func<TSource, bool>>。

复制代码
//正确的代码
Expression<Func<QuestionFeed, bool>> predicate=null;
if (type == 1)
{
    predicate = f => f.FeedID == id && f.IsActive == true;
}
else
{
    predicate = f => f.FeedID == id;
}
_questionFeedRepository.Entities.Where(predicate);
复制代码
         
         */

        //ef 动态拼接参数查询

        public void Test()
        {

          //  Linq的嵌套查询：

             //var student = (from class in m.Classes

             //                     where(class.classId==(from student in m.Students where student.student=2 select student.classId).FirstOrDefault())

             //                   select class).ToList();

        //上述代码所查询的是学生编号为 2 的学生的所在班级，其中FirstOrDefault是代表select后面所需要的结果.

            //Func<Std_InspectionContentTableSchema, WhereClip> whereExp = p =>
            //{
            //    var whereClip = WhereClip.All;
            //    if (DangerLevel != null)
            //    {
            //        whereClip = whereClip && p.DangerLevelStatus == DangerLevel.Value;
            //    }
            //    if (Status != null)
            //    {
            //        whereClip = whereClip && p.Status == Status.Value;
            //    }
            //    return whereClip;
            //};



        //  var list = InspectStandardDB.DBContext.Ji_InspectionContent.Select().Where(m => m.ParentId == search.ParentId).Where(whereExp).ToList();

    }
    }
}