﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stori_Redux_2.Models;

namespace Stori_Redux_2.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public List<Book> Get()
        {
            var folderPath = GetAppSettingsValue.GetFolderPath();
            var files = Directory.GetDirectories(folderPath);
            var id = 0;

            List<Book> bookNames = new List<Book>();
            foreach (var path in files)
            {
                var allPath = Directory.GetDirectories(path);
                string fileName;
                if (path != null)
                {
                    fileName = Path.GetFileName(path).ToString();
                    string[] info = fileName.Split('_');

                    bookNames.Add(new Book { Id = id, BookName = info[0], Author = info[1], ChapterNumber = allPath.Count() });
                    id += 1;
                }
            }
            return bookNames;
        }
    }
}
