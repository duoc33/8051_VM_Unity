using System;
using System.IO;
using System.Linq;
    public class FileTool
    {
        #region Func2
        public static byte[] ExtractHexData(string filePath)
        {
            byte[] code = new byte[1024 * 64];
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // 忽略行末尾的回车和换行符
                    line = line.TrimEnd('\r', '\n');

                    // 检查行是否以 ':' 开始
                    if (line.StartsWith(":"))
                    {
                        // 解析每行的字节数组
                        byte[] data = ParseHexRecord(line);
                        //本行的数据长度(不包括起始地址、数据类型和检验) 16进制字节数
                        byte lineBytesCount = data[0];
                        //该行数据的PC地址
                        ushort addr = (ushort)(data[1] << 8 | data[2]);
                        //该行数据类型
                        byte type = data[3];
                        // 检查行的类型是否为数据记录 (类型码为 00)
                        switch (type)
                        {
                            case 0x00:
                                {
                                    for (int i = 0; i < lineBytesCount; i++)
                                    {
                                        code[addr++] = data[i + 4];
                                    }
                                    break;
                                }
                            case 0x01:
                                {
                                    sr.Close();
                                    sr.Dispose();
                                    return code;
                                }
                            default:
                                UnityEngine.Debug.Log("无该数据类型");
                                break;
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("no : starts");
                        return null;
                    }
                }
            }
            // 返回结果字节数组
            return code;
        }
        private static byte[] ParseHexRecord(string line)
        {
            // 解析十六进制字符串，跳过行首的冒号
            //生成指定范围内的整数的序列。 Enumerable.Range
            byte[] data = Enumerable.Range(0, line.Length / 2 - 1)
                                     .Select(i => Convert.ToByte(line.Substring(i * 2 + 1, 2), 16))
                                     .ToArray();
            return data;
        }
    #endregion

}
