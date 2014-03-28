// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/AvatarController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Dto;
using Application.MainBoundedContext.UserModule;
using Dev.Comm;
using Dev.Comm.Web;
using Dev.Comm.Web.Mvc.Filter;
using Dev.Framework.FileServer;
using WebMatrix.WebData;

namespace CASServer.Controllers
{
    [ActionAllowCrossSiteJson]
    public class AvatarController : Controller
    {
        #region Readonly & Static Fields

        private readonly IImageFile _imagefile;
        private readonly IUserService _userService;

        #endregion

        #region C'tors

        public AvatarController(IImageFile imagefile, IUserService userService)
        {
            this._imagefile = imagefile;
            this._userService = userService;
        }

        #endregion

        #region Instance Methods

        public ActionResult AvataUrl(decimal uid, int type = 4)
        {
            var size = GetSize(type);

            var key = this._userService.GetUserAvatarByUid(uid);
            var url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = this.GetDefaultFace(type);
            }
            else
                url = this._imagefile.GetImageUrl(key, size, size);

            return this.Redirect(url);
        }

        public ActionResult AvataUrlByUserid(int userid, int type = 4)
        {
            var key = this._userService.GetUserAvatar(userid);
            var size = GetSize(type);
            var url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = this.GetDefaultFace(type);
            }
            else
                url = this._imagefile.GetImageUrl(key, size, size);

            return this.Redirect(url);
        }

        [JsonpFilter]
        public ActionResult CurrentUserAvataUrl(int type = 4)
        {
            if (!WebSecurity.IsAuthenticated)
                throw new Exception("未登录的操作");
            var userid = WebSecurity.CurrentUserId;
            var key = this._userService.GetUserAvatar(userid);
            var size = GetSize(type);
            var url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = this.GetDefaultFace(type);
            }
            else
                url = this._imagefile.GetImageUrl(key, size, size);

            return this.Json(url, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult FlashFaceUpload()
        {
            var uid = WebSecurity.CurrentUserId.ToString();
            if (DevRequest.GetString("Filename") != "" && DevRequest.GetString("Upload") != "")
            {
                //string uid = DecodeUid(DevRequest.GetString("input")).Split(',')[0];
                return this.Content(this.UploadTempAvatar(uid));
            }
            if (DevRequest.GetString("avatar1") != "" && DevRequest.GetString("avatar2") != "" &&
                DevRequest.GetString("avatar3") != "")
            {
                //string uid = DecodeUid(DevRequest.GetString("input")).Split(',')[0];
                this.CreateDir(uid);
                if (!(this.SaveAvatar("avatar1", uid) /* && SaveAvatar("avatar2", uid) &&SaveAvatar("avatar3", uid)*/))
                {
                    //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                    return this.Content("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
                }
                //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                return this.Content("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
            }

            return this.Content("");
        }

        public ActionResult FlashIndex()
        {
            return this.View();
        }

        [AllowAnonymous]
        [JsonpFilter]
        public ActionResult FlashJson()
        {
            var content = string.Empty;
            content = this.ViewEngine("FlashIndex", null);
            return this.Json(content, JsonRequestBehavior.AllowGet);
        }

        [ActionAllowCrossSiteJson]
        [JsonpFilter]
        public ActionResult GetAvataUrlByUid(int uid, int type = 4)
        {
            var size = GetSize(type);

            var key = this._userService.GetUserAvatarByUid(uid);
            var url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = this.GetDefaultFace(type);
            }
            else
                url = this._imagefile.GetImageUrl(key, size, size);

            return this.Json(url);
        }

        [ActionAllowCrossSiteJson]
        [JsonpFilter]
        public ActionResult GetAvataUrlByUserid(int userid, int type = 4)
        {
            var key = this._userService.GetUserAvatar(userid);
            var size = GetSize(type);
            var url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = this.GetDefaultFace(type);
            }
            else
                url = this._imagefile.GetImageUrl(key, size, size);

            return this.Json(url);
        }

        //
        // GET: /Avatar/

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Js(string Id)
        {
            return this.JavaScript("show('" + Id + "')");
        }


        [JsonpFilter]
        public ActionResult Json()
        {
            var content = string.Empty;
            content = this.ViewEngine("index");
            return this.Json(content, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        [ValidateInput(false)]
        [JsonpFilter]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveHead(int x, int y, int width, int height, string headFileName)
        {
            if (!WebSecurity.IsAuthenticated)
            {
                return this.Content(ModifiyScript(new BaseState(-1, "用户还未登录")));
            }

            var model = new UploadImageModel();
            model.headFileName = this.Request["headFileName"];
            model.x = Convert.ToInt32(this.Request["x"]);
            model.y = Convert.ToInt32(this.Request["y"]);
            model.width = Convert.ToInt32(this.Request["width"]);
            model.height = Convert.ToInt32(this.Request["height"]);

            var filepath = Path.Combine(this.Server.MapPath("~/avatarImage/temp"), model.headFileName);
            var fileExt = Path.GetExtension(filepath);

            var key = "";

            using (var cutedstream = this.CutAvatar(filepath, model.x, model.y, model.width, model.height, 75L, 180)
                )
            {
                key = this._imagefile.SaveImageFile(cutedstream, model.headFileName, new[]
                                                                                         {
                                                                                             new ImagesSize
                                                                                                 {
                                                                                                     Height = 180,
                                                                                                     Width = 180
                                                                                                 },
                                                                                             new ImagesSize
                                                                                                 {
                                                                                                     Height = 75,
                                                                                                     Width = 75
                                                                                                 }, new ImagesSize
                                                                                                        {
                                                                                                            Height = 50,
                                                                                                            Width = 50
                                                                                                        },
                                                                                             new ImagesSize
                                                                                                 {
                                                                                                     Height = 25,
                                                                                                     Width = 25
                                                                                                 },
                                                                                         });

                cutedstream.Close();
            }

            this._userService.UpdateUserAvatar(WebSecurity.CurrentUserId, key);


            //Dev.Comm.FileUtil.DeleteFile(filepath);

            var state = new BaseState(0, key);

            var script = ModifiyScript(state);
            return this.Content(script);
            return this.Json(new BaseState(0, key), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Test()
        {
            return this.View();
        }

        [ActionAllowCrossSiteJson]
        //[JsonpFilter]
        [Authorize]
        [HttpPost]
        public ActionResult UploadHead(HttpPostedFileBase head) //命名和上传控件name 一样
        {
            BaseState state = null;
            try
            {
                if ((head == null))
                {
                    state = (new BaseState(-1, "无上传文件"));
                }
                else
                {
                    var supportedTypes = new[] {"jpg", "jpeg", "png", "gif", "bmp"};
                    var fileExt = Path.GetExtension(head.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        state = (new BaseState(-1, "文件类型不正确"));
                    }
                    else if (head.ContentLength > 1024*1000*10)
                    {
                        state = (new BaseState(-2, "文件太大"));
                    }
                    else
                    {
                        var r = new Random();
                        var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + "." + fileExt;
                        var filepath = Path.Combine(this.Server.MapPath("~/avatarImage/temp"), filename);
                        head.SaveAs(filepath);

                        var serverfile = HttpServerInfo.BaseUrl + "/avatarImage/temp/" + filename;

                        state = new BaseState(0, serverfile);
                    }
                }
                var jsonstr = JsonConvert.ToJsonStr(state);
                var script =
                    string.Format(
                        "<script type='text/javascript'> if( top.fileuploadcallback ){{ top.fileuploadcallback({0});}}else{{window.alert('不在的图片回调方法');}}</script>",
                        jsonstr);
                return this.Content(script);
            }
            catch (Exception)
            {
                throw;
                return this.Json(new {msg = -3});
            }
        }

        private void CreateDir(string uid)
        {
            var avatarDir = string.Format("/images/upload/avatars/{0}",
                                          uid);
            if (!Directory.Exists(this.Server.MapPath(avatarDir)))
                Directory.CreateDirectory(this.Server.MapPath(avatarDir));
        }

        /// <summary>
        ///   创建缩略图
        /// </summary>
        private MemoryStream CutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, int t)
        {
            var original = Image.FromFile(imgSrc);

            var img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (var gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) ||
                    original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new MemoryStream();
            img.Save(stream, myImageCodecInfo, myEncoderParameters);

            return stream;
        }

        /// <summary>
        ///   创建缩略图
        /// </summary>
        private MemoryStream CutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, string SavePath,
                                       int t)
        {
            var original = Image.FromFile(imgSrc);

            var img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (var gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) ||
                    original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new MemoryStream();
            img.Save(stream, myImageCodecInfo, myEncoderParameters);
            img.Dispose();
            return stream;
        }

        private byte[] FlashDataDecode(string s)
        {
            var r = new byte[s.Length/2];
            var l = s.Length;
            for (var i = 0; i < l; i = i + 2)
            {
                var k1 = (s[i]) - 48;
                k1 -= k1 > 9 ? 7 : 0;
                var k2 = (s[i + 1]) - 48;
                k2 -= k2 > 9 ? 7 : 0;
                r[i/2] = (byte) (k1 << 4 | k2);
            }
            return r;
        }

        private string GetDefaultFace(int type)
        {
            var url = HttpServerInfo.BaseUrl;
            switch (type)
            {
                case 4:
                    url += "/avatarImage/180/default.gif";
                    break;
                case 3:
                    url += "/avatarImage/75/default.gif";

                    break;
                case 2:

                    url += "/avatarImage/50/default.gif";

                    break;
                case 1:

                    url += "/avatarImage/25/default.gif";

                    break;


                default:
                    throw new Exception("Error type");
            }
            return url;
        }

        private bool SaveAvatar(string avatar, string uid)
        {
            var b = this.FlashDataDecode(this.Request[avatar]);
            //if (b.Length == 0)
            //    return false;
            //string size = "";
            //if (avatar == "avatar1")
            //    size = "large";
            //else if (avatar == "avatar2")
            //    size = "medium";
            //else
            //    size = "small";


            //string avatarFileName = string.Format("/images/upload/avatars/{0}/{1}.jpg",
            //    uid, size);
            //FileStream fs = new FileStream(Server.MapPath(avatarFileName), FileMode.Create);
            //fs.Write(b, 0, b.Length);
            //fs.Close();

            var key = "";


            key = this._imagefile.SaveImageFile(b, "headFileName", new[]
                                                                       {
                                                                           new ImagesSize
                                                                               {
                                                                                   Height = 180,
                                                                                   Width = 180
                                                                               },
                                                                           new ImagesSize
                                                                               {
                                                                                   Height = 75,
                                                                                   Width = 75
                                                                               }, new ImagesSize
                                                                                      {
                                                                                          Height = 50,
                                                                                          Width = 50
                                                                                      },
                                                                           new ImagesSize
                                                                               {
                                                                                   Height = 25,
                                                                                   Width = 25
                                                                               },
                                                                       });

            //    cutedstream.Close();
            //}


            this._userService.UpdateUserAvatar(WebSecurity.CurrentUserId, key);

            return true;
        }

        private string UploadTempAvatar(string uid)
        {
            var filename = uid + ".jpg";

            var root = HttpServerInfo.BaseUrl;

            var uploadUrl = root + "/images/upload/avatars";
            var uploadDir = this.Server.MapPath("/images/upload/avatars");
            if (!Directory.Exists(uploadDir + "/temp"))
                Directory.CreateDirectory(uploadDir + "/temp");

            filename = "/temp/" + filename;
            if (this.Request.Files.Count > 0)
            {
                this.Request.Files[0].SaveAs(uploadDir + filename);
            }

            var serverfile = HttpServerInfo.BaseUrl + "/avatarImage/temp/" + filename;

            return uploadUrl + filename;
        }

        private string ViewEngine(string viewName, string layout = "_Layout4Js")
        {
            string content;
            ViewEngineResult view = null;
            if (!string.IsNullOrEmpty(layout))
                view = ViewEngines.Engines.FindView(this.ControllerContext, viewName, layout);
            else
                view = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
            using (var writer = new StringWriter())
            {
                var context = new ViewContext(this.ControllerContext, view.View, this.ViewData, this.TempData, writer);
                view.View.Render(context, writer);

                writer.Flush();
                content = writer.ToString();
            }
            return content;
        }

        #endregion

        //根据长宽自适应 按原图比例缩放 

        #region Class Methods

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private static int GetSize(int type)
        {
            int size;
            switch (type)
            {
                case 4:
                    size = 180;
                    break;
                case 3:
                    size = 75;
                    break;
                case 2:
                    size = 50;
                    break;
                case 1:
                    size = 25;
                    break;


                default:
                    throw new Exception("Error type");
            }
            return size;
        }

        private static Size GetThumbnailSize(Image original, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double) desiredWidth/original.Width;
            var heightScale = (double) desiredHeight/original.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
                       {
                           Width = (int) (scale*original.Width),
                           Height = (int) (scale*original.Height)
                       };
        }

        private static string ModifiyScript(BaseState state)
        {
            var jsonstr = JsonConvert.ToJsonStr(state);
            var script =
                string.Format(
                    "<script type='text/javascript'> if( top.fileupladmodifycallback ){{ top.fileupladmodifycallback({0});}}else{{window.alert('不在的图片回调方法');}}</script>",
                    jsonstr);
            return script;
        }

        #endregion
    }
}