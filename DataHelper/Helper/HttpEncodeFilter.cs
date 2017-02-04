using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace 你好理工.DataHelper.Helper
{
    public class HttpEncodeFilter:IHttpFilter
    {
        private IHttpFilter filter;
        public HttpEncodeFilter(IHttpFilter filter)
        {
            if(filter==null)
            {
                throw new ArgumentException("InnerFilter can't be null");
            }
            this.filter = filter;
        }
        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(Windows.Web.Http.HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage,HttpProgress>(async (cancellationToken,progress)=>
                   {
                       request.Headers.Add("Custom-Header","CustomRequestValue");
                        HttpResponseMessage response = await filter.SendRequestAsync(request).AsTask(cancellationToken,progress);
                        HttpMediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                       if(String.IsNullOrEmpty(contentType.CharSet))
                       {
                           contentType.CharSet = "gb2312";
                       }
                       cancellationToken.ThrowIfCancellationRequested();
                       response.Headers.Add("Custom-Header", "CustomResponseValue");
                       return response;
                   });
        }

        public void Dispose()
        {
            filter.Dispose();
            //throw new NotImplementedException();
        }
    }
}
