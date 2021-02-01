using Devinno.Communications.Scheduler;
using Devinno.Communications.Setting;
using Devinno.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus.RTU
{
    public class ModbusRTUMaster : MasterScheduler
    {
        #region class : WorkMD
        class WorkMD : MasterScheduler.Work
        {
            public int ResponseCount { get; private set; }

            public WorkMD(int id, byte[] data, int ResponseCount)
                : base(id, data)
            {
                this.ResponseCount = ResponseCount;
            }
        }
        #endregion
        #region class : EventArgs
        #region BitReadEventArgs
        public class BitReadEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int Length { get { return (WorkItem.Data[4] << 8) | WorkItem.Data[5]; } }

            public bool[] ReceiveData { get; private set; }

            public BitReadEventArgs(Work WorkItem, bool[] Datas)
            {
                this.WorkItem = WorkItem;
                this.ReceiveData = Datas;
            }
        }
        #endregion
        #region WordReadEventArgs
        public class WordReadEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int Length { get { return (WorkItem.Data[4] << 8) | WorkItem.Data[5]; } }

            public int[] ReceiveData { get; private set; }

            public WordReadEventArgs(Work WorkItem, int[] Datas)
            {
                this.WorkItem = WorkItem;
                this.ReceiveData = Datas;
            }
        }
        #endregion
        #region BitWriteEventArgs
        public class BitWriteEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public bool WriteValue { get { return ((WorkItem.Data[4] << 8) | WorkItem.Data[5]) == 0xFF00; } }

            public BitWriteEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
        #region WordWriteEventArgs
        public class WordWriteEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int WriteValue { get { return ((WorkItem.Data[4] << 8) | WorkItem.Data[5]); } }

            public WordWriteEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
        #region MultiBitWriteEventArgs
        public class MultiBitWriteEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int Length { get { return ((WorkItem.Data[4] << 8) | WorkItem.Data[5]); } }
            public bool[] WriteValues { get; private set; }

            public MultiBitWriteEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
                #region WriteValues
                List<bool> ret = new List<bool>();
                for (int i = 7; i < WorkItem.Data.Length - 2; i++)
                {
                    var v = WorkItem.Data[i];
                    for (int j = 0; j < 8; j++)
                        if (ret.Count < Length) ret.Add(v.Bit(j));
                }
                WriteValues = ret.ToArray();
                #endregion
            }
        }
        #endregion
        #region MultiWordWriteEventArgs
        public class MultiWordWriteEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int Length { get { return ((WorkItem.Data[4] << 8) | WorkItem.Data[5]); } }
            public int[] WriteValues { get; private set; }

            public MultiWordWriteEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
                #region WriteValues
                List<int> ret = new List<int>();
                for (int i = 7; i < WorkItem.Data.Length - 2; i += 2)
                {
                    ret.Add((WorkItem.Data[i] << 8) | WorkItem.Data[i + 1]);
                }
                WriteValues = ret.ToArray();
                #endregion
            }
        }
        #endregion
        #region WordBitSetEventArgs
        public class WordBitSetEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }
            public int BitIndex { get { return WorkItem.Data[4]; } }
            public bool WriteValue { get { return ((WorkItem.Data[5] << 8) | WorkItem.Data[6]) == 0xFF00; } }

            public WordBitSetEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
        #region TimeoutEventArgs
        public class TimeoutEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }

            public TimeoutEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
        #region CRCErrorEventArgs
        public class CRCErrorEventArgs : EventArgs
        {
            public Work WorkItem { get; private set; }

            public int MessageID { get { return WorkItem.MessageID; } }
            public int Slave { get { return WorkItem.Data[0]; } }
            public ModbusFunction Function { get { return (ModbusFunction)WorkItem.Data[1]; } }
            public int StartAddress { get { return (WorkItem.Data[2] << 8) | WorkItem.Data[3]; } }

            public CRCErrorEventArgs(Work WorkItem)
            {
                this.WorkItem = WorkItem;
            }
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// 통신 속도
        /// </summary>
        public int Baudrate { get => ser.BaudRate; set => ser.BaudRate = value; }
        
        /// <summary>
        /// 통신 포트
        /// </summary>
        public string Port { get => ser.PortName; set => ser.PortName = value; }
        
        /// <summary>
        /// 포트 상태
        /// </summary>
        public override bool IsOpen => ser.IsOpen;
        
        /// <summary>
        /// 자동 시작
        /// </summary>
        public bool AutoStart { get; set; }

        protected override int Available
        {
            get
            {
                try
                {
                    return ser.BytesToRead;
                }
                catch (IOException) { throw new SchedulerStopException(); }
                catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
                catch (InvalidOperationException) { throw new SchedulerStopException(); }
            }
        }
        #endregion

        #region Member Variable
        private SerialPort ser = new SerialPort() { PortName = "COM1", BaudRate = 115200 };
        private Thread thAutoStart;
        #endregion

        #region Event
        public event EventHandler<BitReadEventArgs> BitReadReceived;
        public event EventHandler<WordReadEventArgs> WordReadReceived;
        public event EventHandler<BitWriteEventArgs> BitWriteReceived;
        public event EventHandler<WordWriteEventArgs> WordWriteReceived;
        public event EventHandler<MultiBitWriteEventArgs> MultiBitWriteReceived;
        public event EventHandler<MultiWordWriteEventArgs> MultiWordWriteReceived;
        public event EventHandler<WordBitSetEventArgs> WordBitSetReceived;
        public event EventHandler<TimeoutEventArgs> TimeoutReceived;
        public event EventHandler<CRCErrorEventArgs> CRCErrorReceived;

        public event EventHandler DeviceOpened;
        public event EventHandler DeviceClosed;
        #endregion

        #region Constructor
        public ModbusRTUMaster()
        {
            thAutoStart = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (!IsStart && AutoStart)
                    {
                        _Start();
                    }
                    Thread.Sleep(1000);
                }
            }))
            { IsBackground = true };
            thAutoStart.Start();
        }
        #endregion

        #region Method
        #region Start / Stop
        /// <summary>
        /// 지정한 포트 정보로 통신 시작
        /// </summary>
        /// <param name="data">포트 정보</param>
        /// <returns>시작 여부</returns>
        public bool Start(SerialPortSetting data)
        {
            ser.PortName = data.Port;
            ser.BaudRate = data.Baudrate;
            ser.Parity = data.Parity;
            ser.DataBits = data.DataBit;
            ser.StopBits = data.StopBit;
            return Start();
        }

        /// <summary>
        /// 통신 시작
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            return _Start();
        }

        /// <summary>
        /// 통신 정지
        /// </summary>
        public override void Stop()
        {
            if (AutoStart) throw new Exception("AutoStart가 true일 땐 Start/Stop 을 할 수 없습니다.");
            _Stop();
        }

        private bool _Start()
        {
            bool ret = false;
            if (!IsOpen && !IsStart)
            {
                try
                {
                    ser.Open();
                    DeviceOpened?.Invoke(this, null);

                    ret = StartThread();
                    if (!ret && IsOpen) ser.Close();
                }
                catch (Exception) { }
            }
            return ret;
        }

        private void _Stop() => StopThread();
        #endregion

        #region AutoBitRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 자동으로 읽어오는 기능 ( Read Coil Status )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void AutoBitRead_FC1(int id, int Slave, int StartAddr, int Length) => AutoBitRead(id, Slave, 1, StartAddr, Length);

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 자동으로 읽어오는 기능 ( Read Input Status )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void AutoBitRead_FC2(int id, int Slave, int StartAddr, int Length) => AutoBitRead(id, Slave, 2, StartAddr, Length);
        
        private void AutoBitRead(int id, int Slave, byte fn, int StartAddr, int Length)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = fn;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Length.Byte1();
            data[5] = Length.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            int nResCount = Length / 8;
            if (Length % 8 != 0) nResCount++;
            AddAuto(new WorkMD(id, data, nResCount + 5));
        }
        #endregion
        #region AutoWordRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 자동으로 읽어오는 기능 ( Read Holding Registers )  
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void AutoWordRead_FC3(int id, int Slave, int StartAddr, int Length) => AutoWordRead(id, Slave, 3, StartAddr, Length);

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 자동으로 읽어오는 기능 ( Read Input Registers )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void AutoWordRead_FC4(int id, int Slave, int StartAddr, int Length) => AutoWordRead(id, Slave, 4, StartAddr, Length);
        
        private void AutoWordRead(int id, int Slave, byte fn, int StartAddr, int Length)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = fn;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Length.Byte1();
            data[5] = Length.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            AddAuto(new WorkMD(id, data, Length * 2 + 5));
        }
        #endregion

        #region ManualBitRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 읽어오는 기능 ( Read Coil Status )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void ManualBitRead_FC1(int id, int Slave, int StartAddr, int Length) => ManualBitRead(id, Slave, 1, StartAddr, Length);

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 읽어오는 기능 ( Read Input Status )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void ManualBitRead_FC2(int id, int Slave, int StartAddr, int Length) => ManualBitRead(id, Slave, 2, StartAddr, Length);
        
        private void ManualBitRead(int id, int Slave, byte fn, int StartAddr, int Length)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = fn;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Length.Byte1();
            data[5] = Length.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            int nResCount = Length / 8;
            if (Length % 8 != 0) nResCount++;
            AddManual(new WorkMD(id, data, nResCount + 5));
        }
        #endregion
        #region ManualWordRead
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 읽어오는 기능 ( Read Holding Registers ) 
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void ManualWordRead_FC3(int id, int Slave, int StartAddr, int Length) => ManualWordRead(id, Slave, 3, StartAddr, Length);

        /// <summary>
        /// 지정한 국번의 연속되는 주소에 워드값을 읽어오는 기능 ( Read Input Registers ) 
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Length">개수</param>
        public void ManualWordRead_FC4(int id, int Slave, int StartAddr, int Length) => ManualWordRead(id, Slave, 4, StartAddr, Length);
        
        private void ManualWordRead(int id, int Slave, byte fn, int StartAddr, int Length)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = fn;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Length.Byte1();
            data[5] = Length.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            AddManual(new WorkMD(id, data, Length * 2 + 5));
        }
        #endregion
        #region ManualBitWrite
        /// <summary>
        /// 지정한 국번의 주소에 비트값을 쓰는 기능 ( Force Single Coil )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="val">값</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        public void ManualBitWrite_FC5(int id, int Slave, int StartAddr, bool val, int? repeatCount = null)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;
            int Value = val ? 0xFF00 : 0x0000;

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x05;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Value.Byte1();
            data[5] = Value.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;
            AddManual(new WorkMD(id, data, 8) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
        }
        #endregion
        #region ManualWordWrite
        /// <summary>
        /// 지정한 국번의 주소에 워드값을 쓰는 기능 ( Preset Single Register )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Value">값</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        public void ManualWordWrite_FC6(int id, int Slave, int StartAddr, int Value, int? repeatCount = null)
        {
            byte[] data = new byte[8];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x06;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Value.Byte1();
            data[5] = Value.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;
            AddManual(new WorkMD(id, data, 8) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
            data = null;
        }
        #endregion
        #region ManualMultiBitWrite
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 쓰는 기능 ( Force Multiple Coils )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Value">값</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        public void ManualMultiBitWrite_FC15(int id, int Slave, int StartAddr, bool[] Value, int? repeatCount = null)
        {
            int Length = Value.Length / 8;
            Length += (Value.Length % 8 == 0) ? 0 : 1;

            byte[] data = new byte[9 + Length];
            byte crcHi = 0xff, crcLo = 0xff;


            data[0] = Convert.ToByte(Slave);
            data[1] = 0x0F;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Value.Length.Byte1();
            data[5] = Value.Length.Byte0();
            data[6] = Convert.ToByte(Length);

            for (int i = 0; i < Length; i++)
            {
                byte val = 0;
                int nTemp = 0;
                for (int j = (i * 8); j < Value.Length && j < (i * 8) + 8; j++)
                {
                    if (Value[j])
                        val |= Convert.ToByte(Math.Pow(2, nTemp));
                    nTemp++;
                }
                data[7 + i] = val;
            }

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[data.Length - 2] = crcHi;
            data[data.Length - 1] = crcLo;
            AddManual(new WorkMD(id, data, 8) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
        }
        #endregion
        #region ManualMultiWordWrite
        /// <summary>
        /// 지정한 국번의 연속되는 주소에 비트값을 쓰는 기능 ( Preset Multiple Registers )
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="Value">값</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        public void ManualMultiWordWrite_FC16(int id, int Slave, int StartAddr, int[] Value, int? repeatCount = null)
        {
            byte[] data = new byte[9 + (Value.Length * 2)];
            byte crcHi = 0xff, crcLo = 0xff;

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x10;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = Value.Length.Byte1();
            data[5] = Value.Length.Byte0();
            data[6] = Convert.ToByte(Value.Length * 2);

            for (int i = 0; i < Value.Length; i++)
            {
                data[7 + (i * 2)] = Value[i].Byte1();
                data[8 + (i * 2)] = Value[i].Byte0();
            }

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[data.Length - 2] = crcHi;
            data[data.Length - 1] = crcLo;
            AddManual(new WorkMD(id, data, 8) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
        }
        #endregion
        #region ManualWordBitSet
        /// <summary>
        /// 지정한 국번의 주소에 워드의 특정 비트를 쓰는 기능
        /// </summary>
        /// <param name="id">메시지 ID</param>
        /// <param name="Slave">국번</param>
        /// <param name="StartAddr">국번</param>
        /// <param name="BitIndex">비트 인덱스</param>
        /// <param name="val">값</param>
        /// <param name="repeatCount">실패 시 반복횟수</param>
        public void ManualWordBitSet_FC26(int id, int Slave, int StartAddr, byte BitIndex, bool val, int? repeatCount = null)
        {
            byte[] data = new byte[9];
            byte crcHi = 0xff, crcLo = 0xff;
            int Value = val ? 0xFF00 : 0x0000;

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x1A;
            data[2] = StartAddr.Byte1();
            data[3] = StartAddr.Byte0();
            data[4] = BitIndex;
            data[5] = Value.Byte1();
            data[6] = Value.Byte0();

            ModbusCRC.GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[7] = crcHi;
            data[8] = crcLo;

            AddManual(new WorkMD(id, data, 8) { UseRepeat = repeatCount.HasValue, RepeatCount = (repeatCount.HasValue ? repeatCount.Value : 0) });
        }
        #endregion
        #endregion

        #region Override
        #region OnWrite
        protected override void OnWrite(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                ser.DiscardInBuffer();
                ser.DiscardOutBuffer();
                ser.Write(data, offset, count);
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
        }
        #endregion
        #region OnRead
        protected override int? OnRead(byte[] data, int offset, int count, int timeout)
        {
            try
            {
                ser.ReadTimeout = timeout;
                return ser.Read(data, offset, count);
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
            catch { return null; }
        }
        #endregion
        #region OnFlush
        protected override void OnFlush()
        {
            try
            {
                ser.BaseStream.Flush();
            }
            catch (IOException) { throw new SchedulerStopException(); }
            catch (UnauthorizedAccessException) { throw new SchedulerStopException(); }
            catch (InvalidOperationException) { throw new SchedulerStopException(); }
        }
        #endregion
        #region OnThreadEnd
        protected override void OnThreadEnd()
        {
            if (IsOpen) ser.Close();
            DeviceClosed?.Invoke(this, null);
        }
        #endregion
        #region OnTimeout
        public override void OnTimeout(Work w)
        {
            base.OnTimeout(w);
            TimeoutReceived?.Invoke(this, new TimeoutEventArgs(w));
        }
        #endregion
        #region OnCheckCollectComplete
        protected override bool OnCheckCollectComplete(byte[] data, int count, Work w)
        {
            bool ret = false;
            var v = w as WorkMD;
            if (v != null)
            {
                if (count == v.ResponseCount) ret = true;
            }
            return ret;
        }
        #endregion
        #region OnParsePacket
        protected override void OnParsePacket(byte[] baResponse, int nRecv, Work wi)
        {
            var w = wi as WorkMD;
            if (w != null && nRecv > 2)
            {
                byte crcHi = 0, crcLo = 0;
                ModbusCRC.GetCRC(baResponse, nRecv - 2, ref crcHi, ref crcLo);
                if (crcHi == baResponse[nRecv - 2] && crcLo == baResponse[nRecv - 1])
                {
                    ModbusFunction Func = (ModbusFunction)baResponse[1];
                    switch (Func)
                    {
                        case ModbusFunction.BITREAD_F1:
                        case ModbusFunction.BITREAD_F2:
                            #region BitRead
                            {
                                int ByteCount = baResponse[2];
                                int Length = ((w.Data[4] << 8) | w.Data[5]);
                                byte[] baData = new byte[ByteCount];
                                Array.Copy(baResponse, 3, baData, 0, ByteCount);
                                BitArray ba = new BitArray(baData);

                                bool[] bd = new bool[Length];
                                for (int i = 0; i < ba.Length && i < Length; i++) bd[i] = ba[i];

                                BitReadReceived?.Invoke(this, new BitReadEventArgs(w, bd));
                            }
                            #endregion
                            break;
                        case ModbusFunction.WORDREAD_F3:
                        case ModbusFunction.WORDREAD_F4:
                            #region WordRead
                            {
                                int ByteCount = baResponse[2];
                                int[] data = new int[ByteCount / 2];
                                for (int i = 0; i < data.Length; i++) data[i] = Convert.ToUInt16(baResponse[3 + (i * 2)] << 8 | baResponse[4 + (i * 2)]);

                                WordReadReceived?.Invoke(this, new WordReadEventArgs(w, data));
                            }
                            #endregion
                            break;
                        case ModbusFunction.BITWRITE_F5:
                            #region BitWrite
                            {
                                BitWriteReceived?.Invoke(this, new BitWriteEventArgs(w));
                            }
                            #endregion
                            break;
                        case ModbusFunction.WORDWRITE_F6:
                            #region WordWrite
                            {
                                WordWriteReceived?.Invoke(this, new WordWriteEventArgs(w));
                            }
                            #endregion
                            break;
                        case ModbusFunction.MULTIBITWRITE_F15:
                            #region MultiBitWrite
                            {
                                MultiBitWriteReceived?.Invoke(this, new MultiBitWriteEventArgs(w));
                            }
                            #endregion
                            break;
                        case ModbusFunction.MULTIWORDWRITE_F16:
                            #region MultiWordWrite
                            {
                                MultiWordWriteReceived?.Invoke(this, new MultiWordWriteEventArgs(w));
                            }
                            #endregion
                            break;
                        case ModbusFunction.WORDBITSET_F26:
                            #region WordBitSet
                            {
                                WordBitSetReceived?.Invoke(this, new WordBitSetEventArgs(w));
                            }
                            #endregion
                            break;
                    }
                }
                else CRCErrorReceived?.Invoke(this, new CRCErrorEventArgs(w));
            }
        }
        #endregion
        #endregion
    }
}
