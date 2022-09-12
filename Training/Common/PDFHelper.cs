using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Trainning.Common
{
    public class PDFHelper
    {
        /// <summary>
        /// 将Html文字 输出到PDF档里
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            //A4纸规格尺寸：210mm×297mm,1英寸=2.54厘米
            //当分辨率是120像素/英寸时，A4纸像素长宽分别是2105×1487；
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Rectangle pageSize = new Rectangle(826, 1169);
            Document doc = new Document(pageSize);
            //  doc.SetMargins(80, 80, 87, 87);     //设置内容边距
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件预设开档时的缩放为100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //开启Document文件 
            doc.Open();
            //#region pdf文件添加LOGO
            //string logoPath = Environment.CurrentDirectory+("/content/images/logo.png");
            //Image logo = Image.GetInstance(logoPath);
            //float scalePercent = 10f;       //图片缩放比例
            //float percentX = 60f;
            //float percentY = 250f;
            //logo.ScalePercent(scalePercent);
            //logo.SetAbsolutePosition(percentX, doc.PageSize.Height - percentY);
            //doc.Add(logo);
            //#endregion
            //使用XMLWorkerHelper把Html parse到PDF档里
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //将pdfDest设定的资料写到PDF档
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();
            //回传PDF档案 
            return outputStream.ToArray();
        }

    }

    public class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");//arial unicode MS是完整的unicode字型。
        private static readonly string kaiuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "KAIU.TTF");//标楷体
        private static readonly string simsunPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "simsun.ttc,1");//新宋体
        private static readonly string msyhPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "msyh.ttc");//微软雅黑
        private static readonly string stPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "STSONG.TTF");//宋体

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
        {
            //采用默认宋体
            BaseFont baseFont = BaseFont.CreateFont(stPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }

}