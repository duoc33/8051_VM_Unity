using System;
using System.IO;
using System.Threading;
using UnityEngine;


public class VM_Runtime : MonoBehaviour
{
    public static VM_Runtime Instance { get; private set; }
    public bool isRunning = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    //@"C:\Users\35754\Desktop\单片机\codefile\Led\template.hex"
    Thread VM_Thread = null;
    string pathInfo = "";
    public void load_progrom(string path)
    {
        if (VM_Thread != null && !pathInfo.Equals(path))
        {
            close_progrom();
        }
        if (pathInfo.Equals(path)) return;
        pathInfo = path;
        isRunning =true;
        VM_Thread = new Thread(test_progrom);
        VM_Thread.Start(path);
        print("开启线程");
    }
    public void close_progrom() 
    {
        isRunning = false;
        if (VM_Thread != null && VM_Thread.IsAlive) 
        {
            VM_8051_Mono.Instance.isRunning = false;
            VM_8051_Mono.Instance.Reset();
            VM_Thread.Abort();
            VM_Thread=null;
            print("关闭线程");
            return;
        }
        print("无线程");
    }
    private void test_progrom(object path)
    {
        string path_str = path.ToString();
        if (!File.Exists(path_str))
        {
            return;
        }
        byte[] code = FileTool.ExtractHexData(path_str);
        if (code == null)
        {
            print("hexfile loads failed");
            return;
        }
        VM_8051_Mono.Instance.isRunning = true;
        VM_8051_Mono.Instance.Reset();
        VM_8051_Mono.Instance.LoadProgram(code);
        VM_8051_Mono.Instance.Run(0, 0);
    }
    private void OnDestroy()
    {
        if (VM_Thread != null && VM_Thread.IsAlive)
            VM_Thread.Abort();
    }
}




//    private void Start()
//    {


//#if false
//        for (int i = 0; i < HexFilePath.Length; i++)
//        {
//            VM_8051_Mono.Instance.Reset();
//            byte[] code = FileTool.ExtractHexData(HexFilePath[i]);
//            VM_8051_Mono.Instance.LoadProgram(code);
//            ushort pc;
//            do
//            {
//                pc = VM_8051_Mono.Instance.vm_pc();
//                VM_8051_Mono.Instance.FetchCode();
//                VM_8051_Mono.Instance.ExecuteInstruction();
//                Show_Disa();
//            } while (pc != (VM_8051_Mono.Instance.vm_pc()));
//            VM_8051_Mono.Instance.PrintRegResult();
//        }
//#endif
//    }
//private void Show_Disa()
//{

//    InstructInfo info =VM_8051_Mono.Instance.instr.info;
//    if (VM_8051_Mono.Instance.opcodeDic.ContainsKey(VM_8051_Mono.Instance.instr.opcode))
//    {
//        print($"PC: {VM_8051_Mono.Instance.vm_pc().ToString("x")},Cycles: {VM_8051_Mono.Instance.vm_cycles()}");
//        string opcodeHex = VM_8051_Mono.Instance.instr.opcode.ToString("X");
//        string op0 = VM_8051_Mono.Instance.instr.op0.ToString("X");
//        string op1 = VM_8051_Mono.Instance.instr.op1.ToString("X");
//        if (info.bytes == 1)
//        {
//            print($"opcode 0x{opcodeHex} : ASM_Name {info.opcode_name} ");
//        }
//        else if (info.bytes == 2)
//        {
//            print($"opcode 0x{opcodeHex}: ASM_Name {info.opcode_name} , opNum0 0x{op0} ");
//        }
//        else
//        {
//            print($"opcode 0x{opcodeHex}: ASM_Name {info.opcode_name} ,opNum0 0x{op0} , opNum1 0x{op1}");
//        }
//    }
//}
//private readonly static string[] HexFilePath =
//{
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t0_nop\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t1_simple\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t2_move_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t2_move_1\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t2_move_2\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t2_move_3\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t3_movc_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t4_movx_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t5_push_pop\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t6_xch\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t7_anl\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t8_orl\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t9_xrl\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t10_cpl_rr_rl\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t11_add_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t11_add_1\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t12_inc\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t13_dec\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t14_subb_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t14_subb_1\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t15_mul_div_da\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t16_jmp_call_ret\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t17_djnz_jz_cjne\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t18_bit_jb_jc\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t19_serial_0\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t19_serial_1\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t20_timer\Objects\code.hex",
//    @"C:\Users\35754\Desktop\单片机\虚拟机\simu8051-master\test\t21_int\Objects\code.hex"
//};
