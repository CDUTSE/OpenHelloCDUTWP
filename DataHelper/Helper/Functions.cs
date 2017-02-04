
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using 你好理工.DataHelper.Model;
using Windows.UI.Xaml.Media;
using Windows.Graphics.Display;

namespace 你好理工.DataHelper.Helper
{
    public static class Functions
    {
        static Windows.ApplicationModel.Resources.ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();

         
        /// <summary>
        /// 将json反序列化为T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T Deserlialize<T>(string jsonText) where T : class
        {
            T result = default(T);
            try
            {
                if (!String.IsNullOrEmpty(jsonText))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
                    result = deserializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonText))) as T;

                }
            }
            catch(Exception e)
            {
                ShowMessage(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 给可转为int类型的string加上一个int值后转回string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="days">要增加的日期数</param>
        public static string AddDays(this string str,int days)
        {
            
            return (int.Parse(str) + days).ToString();
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        public async static void ShowMessage(string content,string title="")
        {
            //try
            //{
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        try
                        {
                            await new MessageDialog(content, title).ShowAsync();
                        }
                        catch(Exception e)
                        {
                            //ShowMessage(e.Message);
                        }
                     });
            //}
            //catch(Exception e)
            //{
            //    ShowMessage(e.Message);
            //}
        }
        /// <summary>
        /// 获取设备唯一识别码
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueDeviceId()
        {
            HardwareToken ht = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            var id = ht.Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(id);
            byte[] bytes = new byte[id.Length];
            dataReader.ReadBytes(bytes);
            string s = BitConverter.ToString(bytes);
            return s.Replace("-", "");
        }
        /// <summary>
        /// 加载字符串
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string LoadResourceString(string resourceName)
        {
            string str = loader.GetString(resourceName);
            return str;
        }

        const string PUBLIC_KEY = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDbESgubbi3LEUxYBJgzQyGHlmC+Y64owSgyW2j2dBnI4MqTMAXGNvxc6vUvRdJtABYJCX+8+kHsqfjpnBlGPpLIHBK
                    XPXaGaKewoP+EA0TEQkeXBJU6UhshBWcPkHsBraXKn+ktKEmElU/PcX34D6wZWuY/GYPyw4GXrUm4wF5yQIDAQAB";
         /// <summary>
        /// WPRT的RSA公钥加密
        /// </summary>
        /// <param name="rawData">源数据</param>
        /// <param name="publicKeyString">公钥</param>
        /// <returns>加密后的数据</returns>
        public static string PublicEncrypt(string rawData)
        {
            try
            {
                /*将文本转换成IBuffer*/
                IBuffer bufferRawData = CryptographicBuffer.ConvertStringToBinary(rawData, BinaryStringEncoding.Utf8);

                /*加密算法提供程序*/
                AsymmetricKeyAlgorithmProvider provider = AsymmetricKeyAlgorithmProvider.OpenAlgorithm
                    (AsymmetricAlgorithmNames.RsaPkcs1);

                /*导入公钥*/
                CryptographicKey publicKey = provider.ImportPublicKey(CryptographicBuffer.DecodeFromBase64String(PUBLIC_KEY));
               
                //加密
                IBuffer result = CryptographicEngine.Encrypt(publicKey, bufferRawData, null);
                byte[] res;
                CryptographicBuffer.CopyToByteArray(result, out res);
                Debug.WriteLine("WinRT公钥加密后："+Convert.ToBase64String(res));
                return Convert.ToBase64String(res);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Encrypt Exception：" + e.StackTrace);
                return rawData;
            }
        }

        const string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDbESgubbi3LEUxYBJgzQyGHlmC"
            + "+Y64owSgyW2j2dBnI4MqTMAXGNvxc6vUvRdJtABYJCX+8+kHsqfjpnBlGPpLIHBK"
            + "XPXaGaKewoP+EA0TEQkeXBJU6UhshBWcPkHsBraXKn+ktKEmElU/PcX34D6wZWuY"
            + "/GYPyw4GXrUm4wF5yQIDAQAB";

        /// <summary>
        /// BouncyCastle的RSA公钥解密
        /// </summary>
        /// <param name="rawData">要解密的数据</param>
        /// <returns>解密完的数据</returns>
       public static string BCPublicDecrypt(string rawData)
        {
            byte[] btPem = Convert.FromBase64String(publicKey);
            int pemModulus = 128, pemPublicExponent = 3;
            byte[] btPemModulus = new byte[128];
            byte[] btPemPublicExponent = new byte[3];
            for (int i = 0; i < pemModulus; i++)
            {
                btPemModulus[i] = btPem[29 + i];
            }
            for (int i = 0; i < pemPublicExponent; i++)
            {
                btPemPublicExponent[i] = btPem[159 + i];
            }
            BigInteger biModulus = new BigInteger(1, btPemModulus);
            BigInteger biExponent = new BigInteger(1, btPemPublicExponent);
            RsaKeyParameters publicParameters = new RsaKeyParameters(false, biModulus, biExponent);
            IAsymmetricBlockCipher eng = new Pkcs1Encoding(new RsaEngine());
            eng.Init(false, publicParameters);
            //已加密的数据
            byte[] encryptedData = Convert.FromBase64String(rawData);
            encryptedData = eng.ProcessBlock(encryptedData, 0, encryptedData.Length);
            string result = Encoding.UTF8.GetString(encryptedData, 0, encryptedData.Length);
            Debug.WriteLine("BC公钥解密后：" + result);
            return result;
        }

        /// <summary>
        /// BouncyCastle的公钥加密
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string BCPublicEncrypt(string rawData)
       {
           byte[] btPem = Convert.FromBase64String(publicKey);
           int pemModulus = 128, pemPublicExponent = 3;
           byte[] btPemModulus = new byte[128];
           byte[] btPemPublicExponent = new byte[3];
           for (int i = 0; i < pemModulus; i++)
           {
               btPemModulus[i] = btPem[29 + i];
           }
           for (int i = 0; i < pemPublicExponent; i++)
           {
               btPemPublicExponent[i] = btPem[159 + i];
           }
           BigInteger biModulus = new BigInteger(1, btPemModulus);
           BigInteger biExponent = new BigInteger(1, btPemPublicExponent);
           RsaKeyParameters publicParameters = new RsaKeyParameters(false, biModulus, biExponent);
           IAsymmetricBlockCipher eng = new Pkcs1Encoding(new RsaEngine());
           eng.Init(true, publicParameters);
           //要加密的数据
           byte[] encryptData = Encoding.UTF8.GetBytes(rawData);
           encryptData = eng.ProcessBlock(encryptData, 0, encryptData.Length);
           string result = Convert.ToBase64String(encryptData);
           Debug.WriteLine("BC公钥加密后：" + result);
           return result;
       }

        /// <summary>
        /// 获取应用版本
        /// </summary>
        /// <returns></returns>
        public static string GetVersionString()
        {
            string appVersion = string.Format("{0}.{1}.{2}.{3}",
                Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build,
                Package.Current.Id.Version.Revision);
            return appVersion;
        }

        /// <summary>
        /// 应用夜间和日间模式
        /// </summary>
        /// <param name="page"></param>
        public static void ApplyDayModel(Page page)
        {
            //如果是夜间模式
            if (Settings.Instance.NightMode.Equals("True"))
            {
                page.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                page.RequestedTheme = ElementTheme.Light;
            }
        }

        /// <summary>
        /// 判断有没有连上WiFi
        /// </summary>
        public static bool CheckIsWiFi()
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile != null)
            {
                if (internetConnectionProfile.IsWlanConnectionProfile)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否连了网
        /// </summary>
        /// <returns></returns>
        public static bool CheckNetWork()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            //connectionProfile 为null说明没连网
            return (connectionProfile != null);
        }

        /// <summary>
        /// 判断LocalFolder中是否存在某个文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public async static Task<bool> IsFileExists(string fileName)
        {
            try
            {
                 await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            }
            catch(Exception )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 在UI对象中查找一个子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static T FindChildOfType<T>(DependencyObject root) where T:class
        {
            //创建一个队列结构来存放可视化树的对象
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            //循环查找类型
            while(queue.Count>0)
            {
                DependencyObject current = queue.Dequeue();
                //查找子节点的对象类型
                for(int i = VisualTreeHelper.GetChildrenCount(current)-1;0<=i;i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if(typedChild!=null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        /// <summary>
        /// 需要绑定Dialog
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async static Task<bool> ShowMessageDialogWithChoose(string content,string title="需要先关联账号")
        {
            MessageDialog md = new MessageDialog(content,title);
            md.Commands.Add(new UICommand() { Label = "立即绑定", Id = 0 });
            md.Commands.Add(new UICommand(){Label="暂不绑定",Id=1});

            var result = await md.ShowAsync();
            if (result != null && result.Id.Equals(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否大屏(高>480)
        /// </summary>
        /// <returns></returns>
        public static bool IsLargetScreen()
        {
            var ppi = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            return ppi * Window.Current.Bounds.Height > 800;
        }
    }
}
