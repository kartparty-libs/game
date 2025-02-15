using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using UnityEngine;
using Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utility
{
    /// <summary>
    /// 通用工具方法
    /// </summary>
    public class UtilityMethod
    {
        /// <summary>
        /// 获取mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            NetworkInterface[] data = NetworkInterface.GetAllNetworkInterfaces();
            if (data.Length > 0)
            {
                return data[0].GetPhysicalAddress().ToString();
            }
            else
            {
                return "";
            }
        }

        //JObject转JArray （适用于有序JObject）
        public static JArray JObject2JArray(JObject o)
        {
            JArray a = new JArray();

            foreach (var item in o)
            {
                var val = item.Value;
                if (val.GetType() == typeof(JObject))
                {
                    val = JObject2JArray((JObject)val);
                }
                a.Add(val);
            }
            return a;
        }

        /// <summary>
        /// 拷贝List<object>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<object> CopyObjectList(List<object> list)
        {
            List<object> ret = null;
            if (list != null)
            {
                ret = new List<object>();
                for (int i = 0; i < list.Count; i++)
                {
                    ret.Add(list[i]);
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取当前线程Id
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 同步方式Http读取数据
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <returns></returns>
        public static byte[] HttpReadBySync(string i_sDestUrl, out int i_nStatusCode)
        {
            try
            {
                HttpWebRequest pHttpWebRequest = HttpWebRequest.Create(i_sDestUrl) as HttpWebRequest;
                pHttpWebRequest.KeepAlive = false;
                pHttpWebRequest.ContentType = "application/octet-stream";
                pHttpWebRequest.Timeout = 300000;
                pHttpWebRequest.Proxy = WebRequest.DefaultWebProxy;
                pHttpWebRequest.UseDefaultCredentials = true;

                using (HttpWebResponse pHttpWebResponse = pHttpWebRequest.GetResponse() as HttpWebResponse)
                {
                    i_nStatusCode = (int)pHttpWebResponse.StatusCode;
                    using (Stream pStream = pHttpWebResponse.GetResponseStream())
                    {
                        int nLength = (int)pStream.Length;
                        int nBegin = 0;
                        byte[] pBytes = new byte[nLength];
                        while (true)
                        {
                            int nCount = pStream.Read(pBytes, nBegin, nLength - nBegin);
                            if (nCount > 0)
                            {
                                nBegin += nCount;
                            }
                            else
                            {
                                break;
                            }
                        }

                        pHttpWebRequest.Abort();
                        pHttpWebResponse.Close();
                        pStream.Close();
                        return pBytes;
                    }
                }
            }
            catch (Exception pException)
            {
                WebException pWebException = pException as WebException;
                if (pWebException != null)
                {
                    i_nStatusCode = (int)pWebException.Status;
                }
                else
                {
                    i_nStatusCode = -1;
                }
                Debug.LogError("HttpReadBySync.Exception" + " -> DestUrl = " + i_sDestUrl + " -> Exception = " + pException.ToString());
                return null;
            }
        }

        /// <summary>
        /// 异步方式Http读取数据
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <returns></returns>
        public static async Task<byte[]> HttpReadByAsync(string i_sDestUrl)
        {
            return await Task<byte[]>.Run(delegate ()
            {
                return HttpReadBySync(i_sDestUrl, out int i_nStatusCode);
            });
        }

        /// <summary>
        /// 回调方式Http读取数据
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void HttpReadByCallBack(string i_sDestUrl, Action<byte[]> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(HttpReadBySync(i_sDestUrl, out int i_nStatusCode));
                }
            });
        }

        /// <summary>
        /// 同步方式写入文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_pBytes"></param>
        /// <returns></returns>
        public static bool WriteFileBySync(string i_sDestFilePath, byte[] i_pBytes)
        {
            try
            {
                using (FileStream m_pFileStream = new FileStream(i_sDestFilePath, FileMode.CreateNew))
                {
                    m_pFileStream.Write(i_pBytes, 0, i_pBytes.Length);
                    m_pFileStream.Flush();
                    m_pFileStream.Close();
                    return true;
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("WriteFileBySync.Exception" + " -> DestFilePath = " + i_sDestFilePath + " -> Exception = " + pException.ToString());
                return false;
            }
        }

        /// <summary>
        /// 异步方式写入文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_pBytes"></param>
        /// <returns></returns>
        public static async Task<bool> WriteFileByAsync(string i_sDestFilePath, byte[] i_pBytes)
        {
            return await Task<bool>.Run(delegate ()
            {
                return WriteFileBySync(i_sDestFilePath, i_pBytes);
            });
        }

        /// <summary>
        /// 回调方式写入文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_pBytes"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void WriteFileByCallBack(string i_sDestFilePath, byte[] i_pBytes, Action<bool> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(WriteFileBySync(i_sDestFilePath, i_pBytes));
                }
            });
        }

        /// <summary>
        /// 同步方式Http下载文件
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <param name="i_sToFilePath"></param>
        /// <returns></returns>
        public static bool HttpDownloadBySync(string i_sDestUrl, string i_sToFilePath)
        {
            try
            {
                byte[] pBytes = HttpReadBySync(i_sDestUrl, out int i_nStatusCode);
                if (pBytes != null)
                {
                    return WriteFileBySync(i_sToFilePath, pBytes);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("HttpDownloadBySync.Exception" + " -> DestUrl = " + i_sDestUrl + " -> ToFilePath = " + i_sToFilePath + " -> Exception = " + pException.ToString());
                return false;
            }
        }

        /// <summary>
        /// 异步方式Http下载文件
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <param name="i_sToFilePath"></param>
        /// <returns></returns>
        public static async Task<bool> HttpDownloadByAsync(string i_sDestUrl, string i_sToFilePath)
        {
            return await Task<bool>.Run(delegate ()
            {
                return HttpDownloadBySync(i_sDestUrl, i_sToFilePath);
            });
        }

        /// <summary>
        /// 回调方式Http下载文件
        /// </summary>
        /// <param name="i_sDestUrl"></param>
        /// <param name="i_sToFilePath"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void HttpDownloadByCallBack(string i_sDestUrl, string i_sToFilePath, Action<bool> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(HttpDownloadBySync(i_sDestUrl, i_sToFilePath));
                }
            });
        }

        /// <summary>
        /// 同步方式读文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileBySync(string i_sDestFilePath)
        {
            try
            {
                if (!File.Exists(i_sDestFilePath))
                {
                    Debug.LogError("ReadFileBySync" + " -> DestFilePath Is Not Exists! -> " + i_sDestFilePath);
                    return null;
                }
                using (FileStream m_pFileStream = new FileStream(i_sDestFilePath, FileMode.Open))
                {
                    int nLength = (int)m_pFileStream.Length;
                    byte[] pBytes = new byte[nLength];
                    m_pFileStream.Read(pBytes, 0, nLength);
                    m_pFileStream.Close();
                    return pBytes;
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("ReadFileBySync.Exception" + " -> DestFilePath = " + i_sDestFilePath + " -> Exception = " + pException.ToString());
                return null;
            }
        }

        /// <summary>
        /// 异步方式读文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadFileByAsync(string i_sDestFilePath)
        {
            return await Task<byte[]>.Run(delegate ()
            {
                return ReadFileBySync(i_sDestFilePath);
            });
        }
        /// <summary>
        /// 同步方式读文件并反序列化
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static String ReadFileBySync2Serialize(string i_sDestFilePath)
        {
            try
            {
                if (!File.Exists(i_sDestFilePath))
                {
                    Debug.LogError("ReadFileBySync" + " -> DestFilePath Is Not Exists! -> " + i_sDestFilePath);
                    return null;
                }
                using (FileStream m_pFileStream = new FileStream(i_sDestFilePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    string Content = (string)formatter.Deserialize(m_pFileStream);
                    m_pFileStream.Close();
                    return Content;
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("ReadFileBySync.Exception" + " -> DestFilePath = " + i_sDestFilePath + " -> Exception = " + pException.ToString());
                return null;
            }
        }
        /// <summary>
        /// 异步方式读文件并反序列化
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static async Task<String> ReadFileByAsync2Serialize(string i_sDestFilePath)
        {
            return await Task<String>.Run(delegate ()
            {
                return ReadFileBySync2Serialize(i_sDestFilePath);
            });
        }

        /// <summary>
        /// 回调方式读文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void ReadFileByCallBack(string i_sDestFilePath, Action<byte[]> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(ReadFileBySync(i_sDestFilePath));
                }
            });
        }

        /// <summary>
        /// 同步方式读文本文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static string ReadTextFileBySync(string i_sDestFilePath)
        {
            try
            {
                if (!File.Exists(i_sDestFilePath))
                {
                    Debug.LogError("ReadTextFileBySync" + " -> DestFilePath Is Not Exists! -> " + i_sDestFilePath);
                    return null;
                }
                using (StreamReader m_pStreamReader = new StreamReader(i_sDestFilePath))
                {
                    string sResult = m_pStreamReader.ReadToEnd();
                    m_pStreamReader.Close();
                    return sResult;
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("ReadTextFileBySync.Exception" + " -> DestFilePath = " + i_sDestFilePath + " -> Exception = " + pException.ToString());
                return null;
            }
        }

        /// <summary>
        /// 异步方式读文本文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <returns></returns>
        public static async Task<string> ReadTextFileByAsync(string i_sDestFilePath)
        {
            return await Task<string>.Run(delegate ()
            {
                return ReadTextFileBySync(i_sDestFilePath);
            });
        }

        /// <summary>
        /// 回调方式读文本文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void ReadTextFileByCallBack(string i_sDestFilePath, Action<string> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(ReadTextFileBySync(i_sDestFilePath));
                }
            });
        }

        /// <summary>
        /// 同步方式复制文件
        /// </summary>
        /// <param name="i_sFromFilePath"></param>
        /// <param name="i_sToFilePath"></param>
        /// <returns></returns>
        public static bool CopyFileBySync(string i_sFromFilePath, string i_sToFilePath)
        {
            try
            {
                if (!File.Exists(i_sFromFilePath))
                {
                    Debug.LogError("CopyFileBySync" + " -> FromFilePath = " + i_sFromFilePath);
                    return false;
                }
                // File.Copy(i_sFromFilePath, i_sToFilePath, true);
                // return true;
                using (FileStream m_pFromFileStream = new FileStream(i_sFromFilePath, FileMode.Open))
                {
                    using (FileStream m_pToFileStream = new FileStream(i_sToFilePath, FileMode.CreateNew))
                    {
                        m_pFromFileStream.CopyTo(m_pToFileStream);
                        m_pFromFileStream.Close();
                        m_pToFileStream.Close();
                        return true;
                    }
                }
            }
            catch (Exception pException)
            {
                Debug.LogError("CopyFileBySync.Exception" + " -> FromFilePath = " + i_sFromFilePath + " -> ToFilePath = " + i_sToFilePath + " -> Exception = " + pException.ToString());
                return false;
            }
        }

        /// <summary>
        /// 异步方式复制文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_pBytes"></param>
        /// <returns></returns>
        public static async Task<bool> CopyFileByAsync(string i_sFromFilePath, string i_sToFilePath)
        {
            return await Task<bool>.Run(delegate ()
            {
                return CopyFileBySync(i_sFromFilePath, i_sToFilePath);
            });
        }

        /// <summary>
        /// 回调方式复制文件
        /// </summary>
        /// <param name="i_sDestFilePath"></param>
        /// <param name="i_pBytes"></param>
        /// <param name="i_fCallBack"></param>
        /// <returns></returns>
        public static async void CopyFileByCallBack(string i_sFromFilePath, string i_sToFilePath, Action<bool> i_fCallBack)
        {
            await Task.Run(delegate ()
            {
                if (i_fCallBack != null)
                {
                    i_fCallBack.Invoke(CopyFileBySync(i_sFromFilePath, i_sToFilePath));
                }
            });
        }

        // 获取文件MD5值
        public static string GetFileContentMD5(string i_sFile)
        {
            if (!File.Exists(i_sFile))
            {
                return string.Empty;
            }

            FileStream pFileStream = new FileStream(i_sFile, FileMode.Open);
            System.Security.Cryptography.MD5 pMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] pBytes = pMD5.ComputeHash(pFileStream);
            pFileStream.Close();
            pFileStream.Dispose();

            StringBuilder sStringBuilderOfMD5 = new StringBuilder(32);
            for (int i = 0; i < pBytes.Length; i++)
            {
                string sAppend = pBytes[i].ToString("x2");
                string sMD5 = sStringBuilderOfMD5.ToString();
                sStringBuilderOfMD5.Append(sAppend);
            }
            return sStringBuilderOfMD5.ToString();
        }

        public static string GetMD5ByStream(Stream i_pStream)
        {
            System.Security.Cryptography.MD5 pMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] pBytesOfMD5 = pMD5.ComputeHash(i_pStream);
            StringBuilder sStringBuilderOfMD5 = new StringBuilder(32);
            for (int i = 0; i < pBytesOfMD5.Length; i++)
            {
                string sAppend = pBytesOfMD5[i].ToString("x2");
                string sMD5 = sStringBuilderOfMD5.ToString();
                sStringBuilderOfMD5.Append(sAppend);
            }
            return sStringBuilderOfMD5.ToString();
        }

        public static string GetMD5ByBytes(byte[] i_pBytes)
        {
            System.Security.Cryptography.MD5 pMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] pBytesOfMD5 = pMD5.ComputeHash(i_pBytes);
            StringBuilder sStringBuilderOfMD5 = new StringBuilder(32);
            for (int i = 0; i < pBytesOfMD5.Length; i++)
            {
                string sAppend = pBytesOfMD5[i].ToString("x2");
                string sMD5 = sStringBuilderOfMD5.ToString();
                sStringBuilderOfMD5.Append(sAppend);
            }
            return sStringBuilderOfMD5.ToString();
        }

        public static Transform GetChildByName(Transform i_pTransform, string i_sChildName)
        {
            if (i_pTransform.name == i_sChildName)
            {
                return i_pTransform;
            }

            for (int i = 0; i < i_pTransform.childCount; i++)
            {
                Transform pChildTransform = GetChildByName(i_pTransform.GetChild(i), i_sChildName);
                if (pChildTransform != null)
                {
                    return pChildTransform;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取朝向某坐标的旋转角度
        /// </summary>
        /// <param name="i_pSelfPos"></param>
        /// <param name="i_pTargetPos"></param>
        /// <returns></returns>
        public static Vector3 GetTurnEuler(Vector3 i_pSelfPos, Vector3 i_pTargetPos)
        {
            return GetTurnEuler(i_pSelfPos, i_pTargetPos, Vector3.up);
        }

        /// <summary>
        /// 获取朝向某坐标的旋转角度
        /// </summary>
        /// <param name="i_pSelfPos"></param>
        /// <param name="i_pTargetPos"></param>
        /// <param name="i_pUpwards"></param>
        /// <returns></returns>
        public static Vector3 GetTurnEuler(Vector3 i_pSelfPos, Vector3 i_pTargetPos, Vector3 i_pUpwards)
        {
            //计算物体在朝向某个向量后的正前方
            Vector3 pForwardDir = i_pTargetPos - i_pSelfPos;
            //计算朝向这个正前方时的物体四元数值
            return Quaternion.LookRotation(pForwardDir, i_pUpwards).eulerAngles;
        }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <param name="i_eEnum"></param>
        /// <typeparam name="FT0"></typeparam>
        /// <returns></returns>
        public static string GetEnumName<FT0>(FT0 i_eEnum)
        {
            return System.Enum.GetName(typeof(FT0), i_eEnum);
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="i_sEnumName"></param>
        /// <typeparam name="FT0"></typeparam>
        /// <returns></returns>
        public static FT0 GetEnum<FT0>(string i_sEnumName)
        {
            return (FT0)System.Enum.Parse(typeof(FT0), i_sEnumName);
        }

        /// <summary>
        /// 将Table格式的字符串转为string数组
        /// </summary>
        /// <param name="i_sStr"></param>
        /// <returns></returns>
        public static string[] GetArrayByString(string i_sStr)
        {
            var nStartStr = i_sStr[0];
            var nEndStr = i_sStr[i_sStr.Length - 1];
            if (nStartStr != '{' || nEndStr != '}')
            {
                return null;
            }
            var sNewStr = i_sStr.Replace(" ", "");
            sNewStr = sNewStr.Substring(1, sNewStr.Length - 2);
            return sNewStr.Split(',');
        }

        /// <summary>
        /// 深层迭代子物体
        /// </summary>
        /// <param name="i_pTransform"></param>
        /// <param name="i_bIncludeSelf"></param>
        /// <param name="i_fCallback"></param>
        public static void DeepIterateChild(Transform i_pTransform, bool i_bIncludeSelf, System.Action<Transform> i_fCallback)
        {
            if (i_bIncludeSelf)
            {
                i_fCallback.Invoke(i_pTransform);
            }

            for (int i = 0; i < i_pTransform.childCount; i++)
            {
                DeepIterateChild(i_pTransform.GetChild(i), true, i_fCallback);
            }
        }

        /// <summary>
        /// 浅层迭代子物体
        /// </summary>
        /// <param name="i_pTransform"></param>
        /// <param name="i_bIncludeSelf"></param>
        /// <param name="i_fCallback"></param>
        public static void SimpleIterateChild(Transform i_pTransform, bool i_bIncludeSelf, System.Action<Transform> i_fCallback)
        {
            if (i_bIncludeSelf)
            {
                i_fCallback.Invoke(i_pTransform);
            }

            for (int i = 0; i < i_pTransform.childCount; i++)
            {
                i_fCallback.Invoke(i_pTransform.GetChild(i));
            }
        }

        /// <summary>
        /// 深层查找子物体
        /// </summary>
        /// <param name="i_pTransform"></param>
        /// <param name="i_bIncludeSelf"></param>
        /// <param name="i_sName"></param>
        /// <returns></returns>
        public static Transform DeepFindChild(Transform i_pTransform, bool i_bIncludeSelf, string i_sName)
        {
            if (i_bIncludeSelf)
            {
                if (i_pTransform.name == i_sName)
                {
                    return i_pTransform;
                }
            }

            for (int i = 0; i < i_pTransform.childCount; i++)
            {
                Transform pTransform = DeepFindChild(i_pTransform.GetChild(i), true, i_sName);
                if (pTransform != null)
                {
                    return pTransform;
                }
            }

            return null;
        }

        /// <summary>
        /// 浅层查找子物体
        /// </summary>
        /// <param name="i_pTransform"></param>
        /// <param name="i_bIncludeSelf"></param>
        /// <param name="i_sName"></param>
        /// <returns></returns>
        public static Transform SimpleFindChild(Transform i_pTransform, bool i_bIncludeSelf, string i_sName)
        {
            if (i_bIncludeSelf)
            {
                if (i_pTransform.name == i_sName)
                {
                    return i_pTransform;
                }
            }

            for (int i = 0; i < i_pTransform.childCount; i++)
            {
                Transform pTransform = i_pTransform.GetChild(i);
                if (pTransform.name == i_sName)
                {
                    return pTransform;
                }
            }

            return null;
        }

        /** 检测点是否在矩形内 (不考虑矩形角度的问题)*/
        public static bool PointInRectangleCheck(Vector3 i_pPoint, Vector3 i_pRectangPos, int i_nLong, int i_nWidth)
        {
            float nX_1 = i_pRectangPos.x - i_nLong / 2f;
            float nX_2 = i_pRectangPos.x + i_nLong / 2f;
            float nZ_1 = i_pRectangPos.z - i_nWidth / 2f;
            float nZ_2 = i_pRectangPos.z + i_nWidth / 2f;
            if (i_pPoint.x > nX_1 && i_pPoint.x < nX_2 && i_pPoint.z > nZ_1 && i_pPoint.z < nZ_2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 规范化角度，使其值在[0,360]
        /// </summary>
        /// <param name="i_nAngle"></param>
        /// <returns></returns>
        public static float NormalizationAngle(float i_nAngle)
        {
            return i_nAngle < 0f ? (i_nAngle % 360f) + 360f : i_nAngle % 360f;
        }

        /// <summary>
        /// 获取当前角度到目标角度的最小角度的目标角度重算值
        /// </summary>
        /// <param name="i_nTarget"></param>
        /// <param name="i_nCur"></param>
        /// <returns></returns>
        public static float GetAngleByMinTurn(float i_nTarget, float i_nCur)
        {

            float nDiff = i_nTarget - i_nCur;
            if (Mathf.Abs(nDiff) > 180f)
            {
                if (nDiff >= 0f)
                {
                    return i_nTarget - 360f;
                }
                else
                {
                    return i_nTarget + 360f;
                }
            }
            return i_nTarget;
        }

        public static bool IsEqualBySingle(float i_nVal0, float i_nVal1, float i_nPrecision = 0.001f)
        {
            if (Mathf.Abs(i_nVal0 - i_nVal1) >= i_nPrecision)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="i_sString">字符串</param>
        /// <param name="i_bShort">MD5长短 false为长(32) true为短(16)</param>
        /// <returns></returns>
        public static string MD5Encrypt(string i_sString, bool i_bShort = false)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider pMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] pHashedDataBytes;
            pHashedDataBytes = pMD5Hasher.ComputeHash(System.Text.Encoding.GetEncoding("gb2312").GetBytes(i_sString));
            System.Text.StringBuilder pStringBuilder = new System.Text.StringBuilder();
            foreach (byte i in pHashedDataBytes)
            {
                pStringBuilder.Append(i.ToString("x2"));
            }
            if (i_bShort)
            {
                return pStringBuilder.ToString().Substring(8, 16);
            }
            else
            {
                return pStringBuilder.ToString();
            }
        }

        public static int GetRandomNumInt(int i_nMin, int i_nMax)
        {
            return UnityEngine.Random.Range(i_nMin, i_nMax + 1);
        }

        public static float GetRandomNumFloat(float i_nMin, float i_nMax)
        {
            return UnityEngine.Random.Range(i_nMin, i_nMax);
        }

        /// <summary>
        /// 验证是否为整数数字
        /// </summary>
        /// <param name="i_sStr"></param>
        /// <returns></returns>
        public static bool IsValidInt(string i_sStr)
        {
            return Regex.IsMatch(i_sStr, "^([0-9]{1,})$");
        }

        /// <summary>
        /// 验证是否为浮点数字（必须带有小数点）
        /// </summary>
        /// <param name="i_sStr"></param>
        /// <returns></returns>
        public static bool IsValidDecimal(string i_sStr)
        {
            return Regex.IsMatch(i_sStr, "^([0-9]{1,}[.][0-9]*)$");
        }

        /// <summary>
        /// 是否朝向某方向
        /// </summary>
        /// <param name="i_pRot"></param>
        /// <returns></returns>
        public static bool IsTurnReachToRot(Transform i_pSelfTransform, Vector3 i_pRot, float i_nError = 1f)
        {
            Vector3 pMyEulerAngles = i_pSelfTransform.eulerAngles;
            i_pRot.y = UtilityMethod.NormalizationAngle(i_pRot.y);
            float nSubY = Mathf.Abs(pMyEulerAngles.y - i_pRot.y);
            return nSubY < i_nError;
        }

        /// <summary>
        /// 获取朝向某坐标的转向角度
        /// </summary>
        /// <param name="i_pPos"></param>
        /// <returns></returns>
        public static float GetTurnAngle(Transform i_pSelfTransform, Vector3 i_pPos)
        {
            Vector3 pCurPos = i_pSelfTransform.position;
            Vector3 pDis = i_pPos - pCurPos;
            float nAngle = Vector3.Angle(pCurPos, pDis);
            Vector3 pCross = Vector3.Cross(i_pSelfTransform.forward, pDis.normalized);
            if (pCross.y < 0)
            {
                nAngle = 360f - nAngle;
            }
            return nAngle;
        }

        /// <summary>
        /// 获取两个平面坐标的角度
        /// </summary>
        /// <param name="i_pFrom"></param>
        /// <param name="i_pTo"></param>
        /// <returns></returns>
        public static float GetTurnAngle(Vector2 i_pFrom, Vector2 i_pTo)
        {
            Vector2 pDirectionNormalized = (i_pTo - i_pFrom).normalized;
            float nAngle = Mathf.Atan2(pDirectionNormalized.y, pDirectionNormalized.x) * Mathf.Rad2Deg;
            if (nAngle < 0) nAngle = 360 + nAngle;
            return nAngle;
        }

        /// <summary>
        /// 获取以某物体为中心的指定角度指定距离的坐标
        /// </summary>
        /// <param name="i_pSelfPostion"></param>
        /// <param name="i_pAxis">旋转轴</param>
        /// <param name="i_nAngle"></param>
        /// <param name="i_nDistance"></param>
        /// <returns></returns>
        public static Vector3 GetPosByAngle(Vector3 i_pSelfPostion, Vector3 i_pAxis, float i_nAngle, float i_nDistance)
        {
            Vector3 pV3 = Quaternion.Euler(0, i_nAngle, 0) * i_pAxis;
            return i_pSelfPostion + pV3.normalized * i_nDistance;
        }

        /// <summary>
        /// 是否在指定点
        /// </summary>
        /// <param name="i_pCurPos"></param>
        /// <param name="i_pTargetPos"></param>
        /// <param name="i_nAccuracy"></param>
        /// <returns></returns>
        public static bool IsAtTargetPos(Vector3 i_pCurPos, Vector3 i_pTargetPos, float i_nAccuracy = 0.1f)
        {
            float nSubX = Mathf.Abs(i_pCurPos.x - i_pTargetPos.x);
            if (nSubX > i_nAccuracy) return false;
            float nSubY = Mathf.Abs(i_pCurPos.y - i_pTargetPos.y);
            if (nSubY > i_nAccuracy) return false;
            float nSubZ = Mathf.Abs(i_pCurPos.z - i_pTargetPos.z);
            if (nSubZ > i_nAccuracy) return false;
            return true;
        }

        /// <summary>
        /// 随机最外环的点
        /// </summary>
        /// <param name="i_pOrigin">圆心点</param>
        /// <param name="i_nRadiusMin">内圆半径</param>
        /// <param name="i_nRadiusMax">外圆半径</param>
        /// <returns></returns>
        public static Vector3 GetRandomRingPoint(Vector3 i_pOrigin, float i_nRadiusMin, float i_nRadiusMax)
        {
            double nAngle = GetRandomNumFloat(0, 1) * Math.PI * 2;
            float nRadius = GetRandomNumFloat(0, 1) * (i_nRadiusMax - i_nRadiusMin) + i_nRadiusMin;
            float nPosX = i_pOrigin.x + nRadius * Mathf.Cos((float)nAngle);
            float nPosY = i_pOrigin.y + nRadius * Mathf.Sin((float)nAngle);
            return new Vector3(nPosX, nPosY, 0);
        }

        /// <summary>
        /// 二次贝塞尔曲线
        /// </summary>
        /// <param name="i_pStart">起点</param>
        /// <param name="i_pEnd">终点</param>
        /// <param name="i_pOffsetPoint">曲线偏移点</param>
        /// <param name="i_nTime"> [0 ~ 1] 由起点缓动到终点</param>
        /// <returns></returns>
        public static Vector3 Bezier(Vector3 i_pStart, Vector3 i_pEnd, Vector3 i_pOffsetPoint, float i_nTime)
        {
            return (1 - i_nTime) * (1 - i_nTime) * i_pStart + 2 * i_nTime * (1 - i_nTime) * i_pOffsetPoint + i_nTime * i_nTime * i_pEnd;
        }

        /// <summary>
        /// 二次贝塞尔曲线长度
        /// </summary>
        /// <param name="i_pStart"></param>
        /// <param name="i_pEnd"></param>
        /// <param name="i_pOffsetPoint"></param>
        /// <param name="i_nPointCount">切点数量越多，长度越精确，消耗越大</param>
        /// <returns></returns>
        public static float BezierLength(Vector3 i_pStart, Vector3 i_pEnd, Vector3 i_pOffsetPoint, int i_nPointCount = 30)
        {
            if (i_nPointCount < 2)
                return 0;

            float length = 0.0f;
            Vector3 lastPoint = Bezier(i_pStart, i_pEnd, i_pOffsetPoint, 0.0f);
            for (int i = 1; i <= i_nPointCount; i++)
            {
                Vector3 point = Bezier(i_pStart, i_pEnd, i_pOffsetPoint, (float)i / (float)i_nPointCount);
                length += Vector3.Distance(point, lastPoint);
                lastPoint = point;
            }
            return length;
        }

        /// <summary>
        /// 匹配时间格式
        /// </summary>
        /// <param name="i_nNum"></param>
        /// <returns></returns>
        public static string ZeroFrontNum(int i_nNum)
        {
            if (i_nNum < 10)
            {
                return "0" + i_nNum;
            }
            return i_nNum.ToString();
        }

        /// <summary>
        /// 时间格式 00:00
        /// </summary>
        /// <param name="nTime"></param>
        /// <returns></returns>
        public static string GetTimeForm_MinSec(int nTime)
        {
            int nMin = (int)Mathf.Floor(nTime / 60);
            int nSec = nTime - nMin * 60;
            return ZeroFrontNum(nMin) + ":" + ZeroFrontNum(nSec);
        }

        /// <summary>
        /// 时间格式 00:00
        /// </summary>
        /// <param name="nTime"></param>
        /// <returns></returns>
        public static string GetTimeForm_MinSec5(int nTime)
        {
            int nHour = (int)Mathf.Floor(nTime / 3600);
            int nMin = nTime - nHour * 3600;
            nMin = Mathf.FloorToInt(nMin / 60);
            int nSec = nTime - nHour * 3600 - nMin * 60;
            return ZeroFrontNum(nHour) + ":" + ZeroFrontNum(nMin) + ":" + ZeroFrontNum(nSec);
        }
        public static string GetTimeForm_Year_Month_Day(double i_nTimeStamp)
        {
            DateTime dateTime = UnixTimeStampToDateTime(i_nTimeStamp);
            return dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day;
        }
        public static DateTime UnixTimeStampToDateTime(double i_nTimeStamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(i_nTimeStamp);
        }
        public static long GetTime()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public static string NumConvert(int i_nVal)
        {
            if (i_nVal >= 10000000)
            {
                return Math.Round((double)(i_nVal / 1000000), 2) + "M";
            }
            else if (i_nVal >= 10000)
            {
                return Math.Round((double)i_nVal / 1000) + "K";
            }
            return "" + i_nVal;
        }

        /// <summary>
        /// 获取方向绕轴旋转后的向量
        /// </summary>
        /// <param name="i_pDirection">需要旋转的方向向量</param>
        /// <param name="i_nAngle">旋转角度</param>
        /// <param name="i_pAxis">轴</param>
        /// <returns></returns>
        public static Vector3 GetRotateAroundAxis(Vector3 i_pDirection, float i_nAngle, Vector3 i_pAxis)
        {
            return Quaternion.AngleAxis(i_nAngle, i_pAxis) * i_pDirection;
        }

        /// <summary>
        /// 通过文件路径获取目录名
        /// </summary>
        /// <param name="i_sFilePath"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(string i_sFilePath)
        {
            int nLastIndex = i_sFilePath.LastIndexOf('/');
            if (nLastIndex != -1)
            {
                return i_sFilePath.Substring(0, nLastIndex);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过文件路径获取文件名称
        /// </summary>
        /// <param name="i_sFilePath"></param>
        /// <returns></returns>
        public static string GetFileName(string i_sFilePath)
        {
            int nLastIndex = i_sFilePath.LastIndexOf('/');
            if (nLastIndex != -1)
            {
                int nBeginIndex = nLastIndex + 1;
                return i_sFilePath.Substring(nBeginIndex, i_sFilePath.Length - nBeginIndex);
            }
            else
            {
                return i_sFilePath;
            }
        }
    }
}
