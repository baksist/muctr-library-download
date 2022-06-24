using System;
using System.IO;
using System.Net;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace GetLibraryBook
{
    class Program
    {
        static void Main(string[] args)
        {
            int bookNo;
            int pages;

            Download(bookNo, pages);
            BuildPDF(bookNo, pages);
        }

        static void Download(string bookNumber, int pages)
        {
            string url = "https://lib.muctr.ru/digital_library_book/" + bookNumber;
            string phpCookie = "PHPSESSID=";
            string libAuthCookie = "library_card_auth_key=";
            string libCardCookie = "library_card_number=";

            var wb = new WebClient();
            wb.Headers.Add(HttpRequestHeader.Cookie, phpCookie + ";" + 
                                                     libAuthCookie + ";" + 
                                                     libCardCookie);

            Directory.CreateDirectory("pages");

            for (int i = 1; i <= pages; i++)
            {
                var currentURL = url + $"/view_page_image?page={i}";
                wb.DownloadFile(currentURL, $"pages/{i}.png");
                Console.WriteLine($"page {i} downloaded");
            }
        }

        static void BuildPDF(string path, int pages)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter("book.pdf"));
            Document doc = new Document(pdf);

            for (int i = 1; i <= pages; i++)
            {
                ImageData imgData = ImageDataFactory.Create(path + $"/{i}.png");
                Image img = new Image(imgData);
                doc.Add(img);
                Console.WriteLine($"page {i} added to pdf");
            }
            doc.Close();
        }
    }
}