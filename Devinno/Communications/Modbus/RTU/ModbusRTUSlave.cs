﻿using Devinno.Communications.Setting;
using Devinno.Data;
using Devinno.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Modbus.RTU
{
    public class ModbusRTUSlave
    {
        #region Properties
        /// <summary>
        /// 국번
        /// </summary>
        public int Slave { get; set; } = 1;
        
        /// <summary>
        /// 통신 속도
        /// </summary>
        public int Baudrate { get => modbus.Baudrate; set => modbus.Baudrate = value; }
        
        /// <summary>
        /// 통신 포트
        /// </summary>
        public string Port { get => modbus.Port; set => modbus.Port = value; }
        
        /// <summary>
        /// 통신 시작 여부
        /// </summary>
        public bool IsStart => modbus.IsStart;
        
        /// <summary>
        /// 포트 상태
        /// </summary>
        public bool IsOpen => modbus.IsOpen;

        /// <summary>
        /// 비트 영역
        /// </summary>
        public Dictionary<int, BitMemories> BitAreas { get; } = new Dictionary<int, BitMemories>();
        
        /// <summary>
        /// 워드 영역
        /// </summary>
        public Dictionary<int, WordMemories> WordAreas { get; } = new Dictionary<int, WordMemories>();

        public SerialPort NativePort => modbus.NativePort;
        #endregion

        #region Event
        public event EventHandler DeviceClosed;
        public event EventHandler DeviceOpened;
        #endregion

        #region Member Variable
        private ModbusRTUSlaveBase modbus;
        #endregion

        #region Constructor
        public ModbusRTUSlave()
        {
            modbus = new ModbusRTUSlaveBase();

            modbus.BitReadRequest += Modbus_BitReadRequest;
            modbus.WordReadRequest += Modbus_WordReadRequest;
            modbus.BitWriteRequest += Modbus_BitWriteRequest;
            modbus.WordWriteRequest += Modbus_WordWriteRequest;
            modbus.MultiBitWriteRequest += Modbus_MultiBitWriteRequest;
            modbus.MultiWordWriteRequest += Modbus_MultiWordWriteRequest;
            modbus.WordBitSetRequest += Modbus_WordBitSetRequest;

            modbus.DeviceOpened += (o, s) => DeviceOpened?.Invoke(this, null);
            modbus.DeviceClosed += (o, s) => DeviceClosed?.Invoke(this, null);
        }
        #endregion

        #region EventHandler
        private void Modbus_BitReadRequest(object sender, ModbusRTUSlaveBase.BitReadRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    var ret = new bool[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        var sidx = args.StartAddress - BaseAddress + i;
                        ret[i] = mem[sidx];
                    }

                    args.ResponseData = ret;
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordReadRequest(object sender, ModbusRTUSlaveBase.WordReadRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    var ret = new int[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        var sidx = args.StartAddress - BaseAddress + i;
                        ret[i] = mem[sidx];
                    }
                    args.ResponseData = ret;
                    args.Success = true;
                }
            }

        }

        private void Modbus_BitWriteRequest(object sender, ModbusRTUSlaveBase.BitWriteRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size)
                {
                    mem[args.StartAddress - BaseAddress] = args.WriteValue;
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordWriteRequest(object sender, ModbusRTUSlaveBase.WordWriteRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size)
                {
                    mem[args.StartAddress - BaseAddress] = args.WriteValue;
                    args.Success = true;
                }
            }
        }

        private void Modbus_MultiBitWriteRequest(object sender, ModbusRTUSlaveBase.MultiBitWriteRequestArgs args)
        {
            foreach (var BaseAddress in BitAreas.Keys)
            {
                var mem = BitAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    for (int i = 0; i < args.WriteValues.Length; i++) mem[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                    args.Success = true;
                }
            }
        }

        private void Modbus_MultiWordWriteRequest(object sender, ModbusRTUSlaveBase.MultiWordWriteRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress + args.Length < BaseAddress + mem.Size)
                {
                    for (int i = 0; i < args.WriteValues.Length; i++) mem[args.StartAddress - BaseAddress + i] = args.WriteValues[i];
                    args.Success = true;
                }
            }
        }

        private void Modbus_WordBitSetRequest(object sender, ModbusRTUSlaveBase.WordBitSetRequestArgs args)
        {
            foreach (var BaseAddress in WordAreas.Keys)
            {
                var mem = WordAreas[BaseAddress];

                if (args.StartAddress >= BaseAddress && args.StartAddress < BaseAddress + mem.Size && (args.BitIndex >= 0 && args.BitIndex < 16))
                {
                    var p = Convert.ToUInt16(Math.Pow(2, args.BitIndex));
                    if (args.WriteValue) mem[args.StartAddress - BaseAddress] |= p;
                    else mem[args.StartAddress - BaseAddress] &= (ushort)~p;

                    args.Success = true;
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 통신 시작
        /// </summary>
        public void Start() => modbus.Start();

        /// <summary>
        /// 지정한 포트 정보로 통신 시작
        /// </summary>
        /// <param name="setting"></param>
        public void Start(SerialPortSetting setting) => modbus.Start(setting);
        
        /// <summary>
        /// 통신 정지
        /// </summary>
        public void Stop() => modbus.Stop();
        #endregion
    }
}
