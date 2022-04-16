using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.image
{
    public class ImageController : SimpleController
    {
        private DataContext DataContext;
        public ImageController(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        [HttpGet, Route("rpc/dms-abe/image/get/{FileName}")]
        public async Task<IActionResult> Get()
        {
            string UrlParams = HttpContext.Request.Query["url"];
            if (!String.IsNullOrEmpty(UrlParams))
            {
                var Result = await LoadImageByUrl(UrlParams);
                string FileName = UrlParams.Split("/").Last();
                if (Result.Item1 == null)
                {
                    return NotFound();
                }
                return Ok(new ImageDTO
                {
                    Name = FileName,
                    Width = Result.Item1.Width,
                    Height = Result.Item1.Height,
                    MimeType = Result.Item2.MimeTypes.FirstOrDefault(),
                });
            }
            return NotFound();
        }

        [HttpGet, Route("rpc/dms-abe/image/crop/{FileName}")]
        public async Task<IActionResult> Crop()
        {
            try
            {
                string UrlParams = HttpContext.Request.Query["url"];
                var Request = HttpContext.Request;
                string WidthParam = Request.Query["width"];
                string HeightParam = Request.Query["height"];
                string XParam = Request.Query["x"];
                string YParam = Request.Query["y"];
                int DestinationWidth = 0;
                int DestinationHeight = 0;
                int SourceX = 0;
                int SourceY = 0;
                if (!String.IsNullOrEmpty(UrlParams))
                {
                    var Result = await LoadImageByUrl(UrlParams);
                    string FileName = UrlParams.Split("/").Last();
                    if (Result.Item1 == null)
                    {
                        return NotFound();
                    }
                    DestinationWidth = Result.Item1.Width; // neu khong chi dinh width thi lay original width
                    DestinationHeight = Result.Item1.Height; // neu khong chi dinh height thi lay original width

                    if (!String.IsNullOrEmpty(WidthParam))
                    {
                        int.TryParse(WidthParam, out DestinationWidth);
                    }
                    if (!String.IsNullOrEmpty(HeightParam))
                    {
                        int.TryParse(HeightParam, out DestinationHeight);
                    }
                    if (!String.IsNullOrEmpty(XParam))
                    {
                        int.TryParse(XParam, out SourceX);
                    }
                    if (!String.IsNullOrEmpty(YParam))
                    {
                        int.TryParse(YParam, out SourceY);
                    }

                    Image Image = CropImage(Result.Item1, SourceX, SourceY, DestinationWidth, DestinationHeight); // crop image
                    IImageFormat Format = Result.Item2; // format of image

                    MemoryStream OutputStream = new MemoryStream();
                    Image.Save(OutputStream, Format); // save to stream

                    Response.Headers.Add("Content-Type", Result
                        .Item2
                        .MimeTypes
                        .FirstOrDefault());
                    Response.Headers.Add("Content-Length", OutputStream
                        .Length
                        .ToString());

                    return File(OutputStream.ToArray(), "application/octet-stream", FileName);
                }
                return NotFound();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        [HttpGet, Route("rpc/dms-abe/image/resize/{FileName}")]
        public async Task<IActionResult> Resize()
        {
            try
            {
                string UrlParams = HttpContext.Request.Query["url"];
                var Request = HttpContext.Request;
                string WidthParam = Request.Query["width"];
                string HeightParam = Request.Query["height"];
                int DestinationWidth = 0;
                int DestinationHeight = 0;
                if (!String.IsNullOrEmpty(UrlParams))
                {
                    var Result = await LoadImageByUrl(UrlParams);
                    string FileName = UrlParams.Split("/").Last();
                    if (Result.Item1 == null)
                    {
                        return NotFound();
                    }
                    DestinationWidth = Result.Item1.Width; // neu khong chi dinh width thi lay original width
                    DestinationHeight = Result.Item1.Height; // neu khong chi dinh height thi lay original width

                    if (!String.IsNullOrEmpty(WidthParam))
                    {
                        int.TryParse(WidthParam, out DestinationWidth);
                    }
                    if (!String.IsNullOrEmpty(HeightParam))
                    {
                        int.TryParse(HeightParam, out DestinationHeight);
                    }

                    Result.Item1.Mutate(x => x
                       .Resize(DestinationWidth, DestinationHeight)
                    ); // resize image
                    IImageFormat Format = Result.Item2; // format of image
                    MemoryStream OutputStream = new MemoryStream();
                    Result.Item1.Save(OutputStream, Format); // save to stream

                    Response.Headers.Add("Content-Type", Result
                        .Item2
                        .MimeTypes
                        .FirstOrDefault());
                    Response.Headers.Add("Content-Length", OutputStream
                        .Length
                        .ToString());

                    return File(OutputStream.ToArray(), "application/octet-stream", FileName);
                }
                return NotFound();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public async Task<(Image, IImageFormat)> LoadImageByUrl(string Url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var RequestUrl = string.Format("{0}{1}", InternalServices.UTILS, Url);
                HttpResponseMessage Response = await httpClient.GetAsync(RequestUrl); // request to utils get image
                Stream InputStream = await Response.Content.ReadAsStreamAsync(); // read as binary data
                return await Image.LoadWithFormatAsync(InputStream);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        private Image CropImage(Image Image, int SourceX, int SourceY, int DestinationWidth, int DestinationHeight)
        {
            Image.Mutate(x => x
                 .Crop(new Rectangle(SourceX, SourceY, DestinationWidth, DestinationHeight))
            );
            return Image;
        }
    }
}
