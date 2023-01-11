# Devinno [![NuGet stable version](https://badgen.net/nuget/v/devinno)](https://nuget.org/packages/devinno)

## 개요
  * 산업용 프로그램에 개발에 필요한 통신 및 기타 기능들을 정리한 라이브러리
    <br />
    <br />  

## 참조
  * [M2MqttDotNetCore](https://github.com/mohaqeq/paho.mqtt.m2mqtt) [1.1.0](https://www.nuget.org/packages/M2MqttDotnetCore/1.1.0)
  * [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis/) [2.6.70](https://www.nuget.org/packages/StackExchange.Redis/2.6.70)
  * [Newtonsoft.Json](https://www.newtonsoft.com/json) [13.0.1](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1)
    <br />
    <br />  

## 목차
  * Devinno.Collection
    * [EventList](#1-1-EventList)
  * Devinno.Communications
    * [CNet](#2-1-CNet)
    * [MC](#2-2-MC)
    * [ModbusRTUMaster](#2-3-ModbusRTUMaster)
    * [ModbusRTUSlave](#2-4-ModbusRTUSlave)
    * [ModbusTCPMaster](#2-5-ModbusTCPMaster)
    * [ModbusTCPSlave](#2-6-ModbusTCPSlave)
    * [MQClient](#2-7-MQClient)
    * [RedisClient](#2-8-RedisClient)
    * [TextCommRTUMaster](#2-9-TextCommRTUMaster)
    * [TextCommRTUSlave](#2-10-TextCommRTUSlave)
    * [TextCommTCPMaster](#2-11-TextCommTCPMaster)
    * [TextCommTCPSlave](#2-12-TextCommTCPSlave)
  * Devinno.Data
    * [INI](#3-1-INI)
    * [Memories](#3-2-Memories)    
    * [Serialize](#3-3-Serialize)
  * Devinno.Extensions
    * [BitExtension](#4-1-BitExtension)
    * [ColorExtension](#4-2-ColorExtension)
  * Devinno.Measure
    * [Chattering](#5-1-Chattering)
    * [StableMeasure](#5-2-StableMeasure)
  * Devinno.Timers
    * [HiResTimer](#6-1-HiResTimer)
  * Devinno.Tools
    * [CollisionTool](#7-1-CollisionTool)
    * [ColorTool](#7-2-ColorTool)
    * [CryptoTool](#7-3-CryptoTool)
    * [LauncherTool](#7-4-LauncherTool)
    * [MathTool](#7-5-MathTool)
  * Devinno.Utils
    * [ExternalProgram](#8-1-ExternalProgram)
    <br />  
    <br />  

## 사용법
### 1 Devinno.Collection
#### 1-1 EventList
리스트의 변화가 발생시 Changed 이벤트가 발생

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var p1 = new Node { Text = "1" };
    var p2 = new Node { Text = "2" };  p1.Nodes.Add(p2);
    var p3 = new Node { Text = "3" };  p2.Nodes.Add(p3);

    var nd = p3;
    while (nd != null) { Console.WriteLine(nd.Text); nd = nd.Parent; }
}

class Node
{
    public Node Parent { get; set; }
    public EventList<Node> Nodes { get; } = new EventList<Node>();
    public string Text { get; set; }

    public Node() => Nodes.Changed += (o, s) => Nodes.ForEach((x) => x.Parent = this);
}
```

* **결과**
```
3 
2 
1 
```

* **설명** 
``` 
자식 노드들의 변화가 생기면 자식 노드의 부모 노드를 자신으로 지정
```
<br />

### 2 Devinno.Communications
#### 2-1. CNet
LS PLC와 통신할 수 있는 CNET 프로토콜

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var cnet = new CNet { Port = "COM3", Baudrate = 115200 };
    cnet.Start();

    cnet.AutoRSB(10, 1, "%DW000", 4);
    cnet.DataReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.Data) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        cnet.ManualWSS(11, 1, "%DW001", DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0331  0030  0000  0000 
Slave:1  0334  0030  0000  0000 
Slave:1  0339  0031  0000  0000 
... 
```

* **설명**  
```
국번 1번의 PLC에서 DW000부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 DW001에 저장
```
<br />

#### 2-2 MC
Mitsubishi PLC와 통신할 수 있는 MC 프로토콜

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mc = new MC { Port = "COM3", Baudrate = 115200 };
    mc.Start();

    mc.AutoWordRead(10, 1, "D000", 4);
    mc.WordDataReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.Data) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mc.ManualWordWrite(11, 1, "D001", DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  002A  000E  0000  0000 
Slave:1  002D  000E  0000  0000 
Slave:1  0030  000E  0000  0000 
... 
```

* **설명**
```    
국번 1번의 PLC에서 D000부터 4개 값을 자동으로 읽어오는 명령 등록
데이터 수신 시 국번과 데이터를 차례대로 출력
1초 간격으로 현재 초를 D001에 저장
```
<br />

#### 2-3 ModbusRTUMaster
산업용 프로토콜 Modbus. ( RTU, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mb = new ModbusRTUMaster { Port = "COM3", Baudrate = 115200 };
    mb.Start();

    mb.AutoWordRead_FC3(10, 1, 0x7000, 4);
    mb.WordReadReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.ReceiveData) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mb.ManualWordWrite_FC6(11, 1, 0x7001, DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0C75  0003  0000  0000 
Slave:1  0C79  0003  0000  0000 
Slave:1  0C7D  0003  0000  0000 
... 
```

* **설명**
```
국번 1번의 PLC에서 0x7000번지 부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 0x7001번지에 저장  
```
<br />

#### 2-4 ModbusRTUSlave
산업용 프로토콜 Modbus. ( RTU, Slave )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var D = new WordMemories("D", 1024);

    var mb = new ModbusRTUSlave { Port = "COM4", Baudrate = 115200, Slave = 1 };
    mb.WordAreas.Add(0x7000, D);
    mb.Start();

    while(true)
    {
        D[0] = unchecked((ushort)(D[0] + 1));
        Thread.Sleep(10);
    }
}
```

* **결과**
```
      
```

* **설명**
```
모드버스 국번을 1번으로 지정  
워드 영역 D를 모드버스 워드영역 0x7000번지로 등록  
주기적으로 D0를 1씩 증가  
```      
<br />

#### 2-5 ModbusTCPMaster  
산업용 프로토콜 Modbus. ( TCP, Master )
      
* **샘플코드**
```csharp
static void Main(string[] args)
{ 
    var mb = new ModbusTCPMaster { RemoteIP = "192.168.0.50" }; ;
    mb.Start();

    mb.AutoWordRead_FC3(10, 1, 0x7000, 4);
    mb.WordReadReceived += (o, s) =>
    {
        if (s.MessageID == 10)
        {
            Console.Write($"Slave:{s.Slave}  ");
            foreach (var v in s.ReceiveData) Console.Write($"{v.ToString("X4")}  ");
            Console.WriteLine("");
        }
    };

    while (true)
    {
        mb.ManualWordWrite_FC6(11, 1, 0x7001, DateTime.Now.Second);
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  0568  0009  0000  0000 
Slave:1  056B  0009  0000  0000 
Slave:1  056E  0009  0000  0000 
...
```

* **설명**
```
192.168.0.50의 PLC에서 0x7000번지 부터 4개 값을 자동으로 읽어오는 명령 등록  
데이터 수신 시 국번과 데이터를 차례대로 출력  
1초 간격으로 현재 초를 0x7001번지에 저장  
```
<br />

#### 2-6 ModbusTCPSlave  
산업용 프로토콜 Modbus. ( TCP, Slave )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var D = new WordMemories("D", 1024);

    var mb = new ModbusTCPSlave { Slave = 1 };
    mb.WordAreas.Add(0x7000, D);
    mb.Start();

    while (true)
    {
        D[0] = unchecked((ushort)(D[0] + 1));
        Thread.Sleep(10);
    }
}
```

* **결과**
```
          
```

* **설명**
```
모드버스 국번을 1번으로 지정  
워드 영역 D를 모드버스 워드영역 0x7000번지로 등록  
주기적으로 D0를 1씩 증가      
```  
<br />

#### 2-7 MQClient  
MQTT Client 래퍼 ( M2MQTT 기반 )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var mqtt = new MQClient { BrokerHostName = "127.0.0.1" };
    mqtt.Start(Guid.NewGuid().ToString());

    mqtt.Subscribe("/time");
    mqtt.Received += (o, s) => { 
        if (s.Topic == "/time") 
            Console.WriteLine(("R:" + Encoding.UTF8.GetString(s.Datas)).PadLeft(50)); 
    };

    Thread.Sleep(1000);

    while(true)
    {
        mqtt.Publish("/time", Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
        Console.WriteLine(("S:" + DateTime.Now.ToString()).PadRight(50));
        System.Threading.Thread.Sleep(1000);
    }
}
```

* **결과**
```
S:2022-10-24 오후 4:45:15 
                            R:2022-10-24 오후 4:45:15 
S:2022-10-24 오후 4:45:16 
                            R:2022-10-24 오후 4:45:16 
S:2022-10-24 오후 4:45:17 
                            R:2022-10-24 오후 4:45:17 
... 
```

* **설명**
```
/test 구독하고 해당 토픽 수신 시 수신되 데이터 콘솔에 출력
1초에 한 번씩 /test에 현재 시간 문자열을 발행
```
<br />

#### 2-8 RedisClient  
Redis Client 래퍼 ( StackExchange.Redis 기반 )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var redis = new RedisClient { Host = "127.0.0.1" };
    redis.Open();
            
    var tmr = new HiResTimer { Interval = 1000, Enabled = true };
    tmr.Elapsed += (o,s)=> redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));
    
    redis.Set("Time", DateTime.Now.ToString("HH:mm:ss.fff"));
    while (true)
    {
        Console.WriteLine(redis.GetString("Time"));
        System.Threading.Thread.Sleep(500);
    }
}
```

* **결과**
```
18:13:38.395
18:13:38.395
18:13:39.396
18:13:39.396
18:13:40.396
18:13:40.396
... 
```

* **설명**
```
타이머를 이용해 1초 마다 Time 에 현재 시간을 넣고
루프에선 500ms 마다 Time 정보를 읽어와 콘솔에 출력
결과를 보면 2개 출력마다 값이 변경됨
```
<br />

#### 2-9 TextCommRTUMaster  
텍스트 송/수신 프로토콜 ( RTU, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommRTUMaster { Port = "COM3", Baudrate = 115200, Interval = 100 };
    comm.Start();

    comm.MessageReceived += (o, s) => Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  Data:{s.Message}");
    comm.AutoSend(11, 1, 1, "");

    while (true)
    {
        comm.ManualSend(12, 1, 2, DateTime.Now.ToString("HH:mm:ss.fff"));
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:38
Slave:1  Command:1  Data:Count:39
Slave:1  Command:1  Data:Count:40
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:41
Slave:1  Command:1  Data:Count:42
Slave:1  Command:1  Data:Count:43
Slave:1  Command:1  Data:Count:44
Slave:1  Command:2  Data:OK 
...   
```

* **설명**
```
커맨드 1을 자동으로 전송하도록 등록  
메시지 수신 시 국번, 커맨드, 메시지를 콘솔로 출력  
1초 주기로 커맨드 2번으로 현재 시간을 전송  
```
<br />

#### 2-10 TextCommRTUSlave  
텍스트 송/수신 프로토콜 ( RTU, Slave )
 
* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommRTUSlave { Port = "COM4", Baudrate = 115200 };
    comm.Start();

    var cnt = 0;
    comm.MessageRequest += (o, s) =>
    {
        if (s.Slave == 1)
        {
            switch (s.Command)
            {
                case 1:
                    cnt++;
                    s.ResponseMessage = $"Count:{cnt}";
                    break;
                case 2:
                    Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  {s.RequestMessage}");
                    s.ResponseMessage = "OK";
                    break;
            }
        }
    };

    while (true) Thread.Sleep(100);
}
```

* **결과**
```
Slave:1  Command:2  20:25:00.612
Slave:1  Command:2  20:25:01.625
Slave:1  Command:2  20:25:02.632
Slave:1  Command:2  20:25:03.642
Slave:1  Command:2  20:25:04.656
Slave:1  Command:2  20:25:05.658
Slave:1  Command:2  20:25:06.670
Slave:1  Command:2  20:25:07.684
...   
```

* **설명**
```
MessageRequest 이벤트에서는 요청 메시지를 처리하고 응답 메시지를 지정함  
커맨드 1에서는 카운터를 증가하고 해당 카운터값을 응답함  
커맨드 2에서는 수신된 요청을 콘솔에 출력하고 OK 응답  
```

#### 2-11 TextCommTCPMaster  
텍스트 송/수신 프로토콜 ( TCP, Master )

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommTCPMaster { RemoteIP = "192.168.0.55", RemotePort = 1234, Interval = 100 };
    comm.Start();

    comm.MessageReceived += (o, s) => Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  Data:{s.Message}");
    comm.AutoSend(11, 1, 1, "");

    while (true)
    {
        comm.ManualSend(12, 1, 2, DateTime.Now.ToString("HH:mm:ss.fff"));
        Thread.Sleep(1000);
    }
}
```

* **결과**
```
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:17
Slave:1  Command:1  Data:Count:18
Slave:1  Command:1  Data:Count:19
Slave:1  Command:2  Data:OK
Slave:1  Command:1  Data:Count:20
Slave:1  Command:1  Data:Count:21
Slave:1  Command:1  Data:Count:22
Slave:1  Command:1  Data:Count:23
...   
```

* **설명**
```
커맨드 1을 자동으로 전송하도록 등록  
메시지 수신 시 국번, 커맨드, 메시지를 콘솔로 출력  
1초 주기로 커맨드 2번으로 현재 시간을 전송  
```
<br />

#### 2-12 TextCommTCPSlave  
텍스트 송/수신 프로토콜 ( TCP, Slave )
 
* **샘플코드**
```csharp
static void Main(string[] args)
{
    var comm = new TextCommTCPSlave { LocalPort = 1234 };
    comm.Start();

    var cnt = 0;
    comm.MessageRequest += (o, s) =>
    {
        if (s.Slave == 1)
        {
            switch (s.Command)
            {
                case 1:
                    cnt++;
                    s.ResponseMessage = $"Count:{cnt}";
                    break;
                case 2:
                    Console.WriteLine($"Slave:{s.Slave}  Command:{s.Command}  {s.RequestMessage}");
                    s.ResponseMessage = "OK";
                    break;
            }
        }
    };

    while (true) Thread.Sleep(100);
}
```

* **결과**
```
Slave:1  Command:2  21:13:05.151
Slave:1  Command:2  21:13:06.170
Slave:1  Command:2  21:13:07.183
Slave:1  Command:2  21:13:08.197
Slave:1  Command:2  21:13:09.211
Slave:1  Command:2  21:13:14.420
Slave:1  Command:2  21:13:15.452
Slave:1  Command:2  21:13:16.455
...   
```

* **설명**
```
MessageRequest 이벤트에서는 요청 메시지를 처리하고 응답 메시지를 지정함  
커맨드 1에서는 카운터를 증가하고 해당 카운터값을 응답함  
커맨드 2에서는 수신된 요청을 콘솔에 출력하고 OK 응답  
```
<br />

### 3 Devinno.Data
#### 3-1 INI  
INI 파일 읽기/쓰기

* **샘플코드**
```csharp
static void Main(string[] args)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        var ini = new INI(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.ini"));

        ini.Write("SectionA", "Test1", "1");
        ini.Write("SectionA", "Test2", "2");
        ini.Write("SectionB", "Test3", "3");
        ini.Write("SectionB", "Test4", "4");
        ini.Write("SectionC", "Test5", "5");
        ini.Write("SectionC", "Test6", "6");

        Console.WriteLine($"[SectionA] Test1 = {ini.Read("SectionA", "Test1")}");
        Console.WriteLine($"[SectionA] Test2 = {ini.Read("SectionA", "Test2")}");
        Console.WriteLine($"[SectionB] Test3 = {ini.Read("SectionB", "Test3")}");
        Console.WriteLine($"[SectionB] Test4 = {ini.Read("SectionB", "Test4")}");
        Console.WriteLine($"[SectionC] Test5 = {ini.Read("SectionC", "Test5")}");
        Console.WriteLine($"[SectionC] Test6 = {ini.Read("SectionC", "Test6")}");
    }

    Console.ReadKey(); 
}
```

* **결과**
```
[SectionA] Test1 = 1
[SectionA] Test2 = 2
[SectionB] Test3 = 3
[SectionB] Test4 = 4
[SectionC] Test5 = 5
[SectionC] Test6 = 6
```

* **설명**  
```
INI 클래스는 Window 환경에서만 동작
위 코드는 값을 쓰고 읽어 화면에 표시
```
<br />      

#### 3-2 Memories  
비트영역 / 워드영역

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var ba = new byte[1024];
    var M = new BitMemories("M", ba);
    var WM = new WordMemories("WM", ba);

    WM[0] = 0x1234;

    Console.WriteLine($"  M Count : {M.Size}");
    Console.WriteLine($" WM Count : {WM.Size}");
    Console.WriteLine("");
    Console.WriteLine($" WM0 = {WM[0].ToString("X4")}");

    for (int i = 15; i >= 0; i--) Console.Write($"M{i}".PadLeft(4));
    Console.WriteLine("");
    for (int i = 15; i >= 0; i--) Console.Write((M[i] ? "1" : "0").PadLeft(4));

    Console.ReadKey();
}
```

* **결과**
```
  M Count : 8192
 WM Count : 512

 WM0 = 1234
 M15 M14 M13 M12 M11 M10  M9  M8  M7  M6  M5  M4  M3  M2  M1  M0
   0   0   0   1   0   0   1   0   0   0   1   1   0   1   0   0
```

* **설명**  
```
동일한 메모리를 공유하는 비트영역과 워드영역
WM0에 0x1234를 대입하면 M0부터 하위비트 매칭 
```
<br />      

#### 3-3 Serialize  
객체 직렬화

* **샘플코드**
```csharp
class Test
{
    public int Number { get; set; }
    public float Real { get; set; }
    public string Text { get; set; }

    public override string ToString() => $"N:{Number};R:{Real};T:{Text}";
}

static void Main(string[] args)
{
    var v = new Test { Number = 25, Real = 36.5F, Text = "Test" };

    var json = Serialize.JsonSerialize(v);
    var text = Serialize.JsonDeserialize<Test>(json);

    Console.WriteLine("# Json");
    Console.WriteLine(json);
    Console.WriteLine("");
    Console.WriteLine("# Text");
    Console.WriteLine(text);

    Console.ReadKey();
}
```

* **결과**
```
# Json
{"Number":25,"Real":36.5,"Text":"Test"}

# Text
N:25;R:36.5;T:Test
```

* **설명**  
```
v를 Json으로 직렬화 시키고 출력
Json을 역직렬화한 객체 출력
```
<br />

### 4 Devinno.Extensions
#### 4-1 BitExtension  
비트 확장

* **샘플코드**
```csharp
static void PR(bool v) => PR(v ? "1" : "0");
static void PR(string v) => Console.Write(v);

static void Main(string[] args)
{
    int n = 0;

    n.Bit15(false); n.Bit14(false); n.Bit13(false); n.Bit12(true);  //0001
    n.Bit11(false); n.Bit10(false); n.Bit9(true); n.Bit8(false);    //0010
    n.Bit7(false); n.Bit6(false); n.Bit5(true); n.Bit4(true);       //0011
    n.Bit3(false); n.Bit2(true); n.Bit1(false); n.Bit0(false);      //0100

    Console.WriteLine("0001 0010 0011 0100 => ");
    Console.WriteLine("0x" + n.ToString("X4"));
    Console.WriteLine(" ");

    n = 0x2580;
    Console.WriteLine("0x2580 => ");
    PR(n.Bit15()); PR(n.Bit14()); PR(n.Bit13()); PR(n.Bit12()); PR(" ");
    PR(n.Bit11()); PR(n.Bit10()); PR(n.Bit9()); PR(n.Bit8()); PR(" ");
    PR(n.Bit7()); PR(n.Bit6()); PR(n.Bit5()); PR(n.Bit4()); PR(" ");
    PR(n.Bit3()); PR(n.Bit2()); PR(n.Bit1()); PR(n.Bit0()); PR(" ");

    Console.ReadKey();
}
```

* **결과**
```
0001 0010 0011 0100 =>
0x1234

0x2580 =>
0010 0101 1000 0000
```

* **설명**  
```
확장 메소드 Bit0 ~ Bit15를 이용하여 비트 읽기/쓰기
```
<br />

#### 4-2 ColorExtension  
색상 확장

* **샘플코드**
```csharp
static string PER(double v) => v.ToString("0%");
static void Main(string[] args)
{
    var c = Color.LimeGreen;
    var h = c.ToHSV();
    Console.WriteLine($"RGB({c.R}, {c.G}, {c.B}) => HSV({h.H}, {PER(h.S)}, {PER(h.V)})");

    var h2 = new HsvColor() { A = 1, H = 200, S = 0.45, V = 0.6 };
    var c2 = h2.ToRGB();
    Console.WriteLine($"HSV({h2.H}, {PER(h2.S)}, {PER(h2.V)}) => RGB({c2.R}, {c2.G}, {c2.B})");

    Console.ReadKey();
}
```

* **결과**
```
RGB(50, 205, 50) => HSV(120, 76%, 80%)
HSV(200, 45%, 60%) => RGB(84, 130, 153)
```

* **설명**  
```
RGB를 HSV로 변환
HSV를 RGB로 변화
```
<br />

### 5 Devinno.Measure
#### 5-1 Chattering  
채터링

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var chat = new Chattering { ChatteringTime = 500 };
    var rnd = new Random();

    chat.StateChanged += (o, s) => Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + (chat.State ? 1 : 0));

    var v = false;
    var v2 = false;
    var ch = false;
    while (true)
    {
        var now = DateTime.Now;

        if (now.Second % 2 == 1) { v = !v; ch = true; }
        else { if (ch) { v2 = !v2; v = v2; ch = false;  } }

        chat.Set(v);

        Thread.Sleep(10);
    }
}
```

* **결과**
```
10:41:22 : 0
10:41:24 : 1
10:41:26 : 0
10:41:28 : 1
10:41:30 : 0
10:41:32 : 1
10:41:34 : 0
10:41:36 : 1
10:41:38 : 0
```

* **설명**  
```
홀수초에는 값이 반전, 짝수초에는 진입 시 반전 후 유지
채터링 상태 변화 시 출력
출력 결과를 보면 짝수 초에만 출력됨
```
<br />

#### 5-2 StableMeasure  
상태값이 지정한 오차치 이내로 유지되면 이벤트 발생 

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var stable = new StableMeasure { MeasureTime = 300, ErrorRange = 1 };
    stable.Measured += (o, s) => Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + stable.Value);

    var v = 0.0;
    var r = new Random();
    while (true)
    {
        var now = DateTime.Now;
        if (now.Second % 5 != 0) v = MathTool.Constrain(v + (r.Next() % 2 == 0 ? 3 : -3), -500, 500);

        stable.Set(v);

        Thread.Sleep(10);
    }
}
```

* **결과**
```
12:03:35 : 4
12:03:40 : -12
12:03:45 : 3
12:03:50 : -6
12:03:55 : -37
12:04:00 : -70
```

* **설명**  
```
상태값을 지속적으로 3씩 변화시키고 5초에 한번씩 상태 유지
현재값이 오차값 1이하로 300ms 이상 유지되면 출력
결과를 보면 5초에 한번씩 출력되는 결과 확인
```
<br />

### 6 Devinno.Timers
#### 6-1 HiResTimer  
비교적 정확한 타이머

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var tmr = new HiResTimer { Interval = 5, Enabled = true };
    tmr.Elapsed += (o, s) => Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

    Console.ReadKey();

    tmr.Enabled = false;
}
```

* **결과**
```
12:58:26.260
12:58:26.265
12:58:26.270
12:58:26.275
12:58:26.280
12:58:26.285
12:58:26.290
12:58:26.295
12:58:26.300
```

* **설명**  
```
5ms 단위로 현재 시간 출력
```
<br />

      
### 7 Devinno.Tools
#### 7-1 CollisionTool  
충돌 검사

* **샘플코드**
```csharp
static void Print(string s, bool v) => Console.WriteLine(s + " : " + (v ? "충돌" : "미충돌"));
static void Main(string[] args)
{
    Print("Rect(0,0,80,20), Point(10,10)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Point(10, 10)));
    Print("Rect(0,0,80,20), Point(100,10)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Point(100, 10)));
    Print("Rect(0,0,80,20), Rect(10,10,30,30)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Rectangle(10, 10, 30, 30)));
    Print("Rect(0,0,80,20), Rect(100,10,30,30)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Rectangle(100, 10, 30, 30)));
    Print("Circle(0,0,80,80), Point(30,30)", CollisionTool.CheckCircle(new Rectangle(0, 0, 80, 80), new Point(30, 30)));
    Print("Circle(0,0,80,80), Point(10,10)", CollisionTool.CheckCircle(new Rectangle(0, 0, 80, 80), new Point(10, 10)));
    Print("Ellipse(0,0,80,30), Ellipse(40,15,80,30)", CollisionTool.CheckEllipse(new Rectangle(0, 0, 80, 30), new Rectangle(40, 15, 80, 30)));
    Print("Ellipse(0,0,80,30), Ellipse(70,15,80,30)", CollisionTool.CheckEllipse(new Rectangle(0, 0, 80, 30), new Rectangle(70, 15, 80, 30)));
    Print("Line(0,0,30,30), Line(10,10,40,40)", CollisionTool.CheckLine(new Point(0, 0), new Point(30, 30), new Point(10, 10), new Point(40, 40)));
    Print("Line(0,0,30,30), Line(40,10,10,40)", CollisionTool.CheckLine(new Point(0, 0), new Point(30, 30), new Point(40, 10), new Point(10, 40)));
    Console.ReadKey();
}
```

* **결과**
```
Rect(0,0,80,20), Point(10,10) : 충돌
Rect(0,0,80,20), Point(100,10) : 미충돌
Rect(0,0,80,20), Rect(10,10,30,30) : 충돌
Rect(0,0,80,20), Rect(100,10,30,30) : 미충돌
Circle(0,0,80,80), Point(30,30) : 충돌
Circle(0,0,80,80), Point(10,10) : 미충돌
Ellipse(0,0,80,30), Ellipse(40,15,80,30) : 충돌
Ellipse(0,0,80,30), Ellipse(70,15,80,30) : 미충돌
Line(0,0,30,30), Line(10,10,40,40) : 미충돌
Line(0,0,30,30), Line(40,10,10,40) : 충돌
```

* **설명**  
```
충돌 여부 출력 
```
<br />

#### 7-2 ColorTool  
색상 코드값 출력 및 색상 혼합

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var c = Color.FromArgb(0, 128, 128);
    Console.WriteLine(ColorTool.GetName(c, ColorCodeType.ARGB));
    Console.WriteLine(ColorTool.GetName(c, ColorCodeType.CodeARGB));
    Console.WriteLine(ColorTool.GetName(c, ColorCodeType.RGB));
    Console.WriteLine(ColorTool.GetName(c, ColorCodeType.CodeRGB));

    var c2 = ColorTool.MixColorAlpha(Color.Red, Color.Blue, 64);
    Console.WriteLine(ColorTool.GetName(c2, ColorCodeType.RGB));
    Console.ReadKey();
}
```

* **결과**
```
255,0,128,128
#FF008080
0,128,128
#008080
191,0,64
```

* **설명**  
```
RGB(0,128,128)의 색상 코드 출력
빨간색과 25% (64/256) 투명도가 적용된 파란색 혼합 시 색상 출력 
```
<br />

#### 7-3 CryptoTool  
암/복호화

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var s = "동해물과 백두산이 마르고 닳도록";
    var k = "1234567890ABCDEF";

    var p1 = CryptoTool.EncodeBase64String(s);
    var r1 = CryptoTool.DecodeBase64String<string>(p1);
    Console.WriteLine("- Base64String");
    Console.WriteLine(p1);
    Console.WriteLine(r1);
    Console.WriteLine("");

    var p2 = CryptoTool.EncryptAES128(s, k);
    var r2 = CryptoTool.DecryptAES128(p2, k);
    Console.WriteLine("- AES128");
    Console.WriteLine(p2);
    Console.WriteLine(r2);
    Console.WriteLine("");

    var p3 = CryptoTool.EncryptAES256(s, k);
    var r3 = CryptoTool.DecryptAES256(p3, k);
    Console.WriteLine("- AES256");
    Console.WriteLine(p3);
    Console.WriteLine(r3);
    Console.WriteLine("");

    Console.ReadKey();
}
```

* **결과**
```
- Base64String
64+Z7ZW066y86rO8IOuwseuRkOyCsOydtCDrp4jrpbTqs6Ag64uz64+E66Gd
동해물과 백두산이 마르고 닳도록

- AES128
vm2dKMAhhsQdfZEE6MLfwKUh3l/MBWpgBuTVvFqrO5jzV2cKDm8xcZZ4beGn+s+6
동해물과 백두산이 마르고 닳도록

- AES256
zDD5Z33LQ5dTB6idpfoqhuAS9kohe2cD3FHJJlGwwkU++1T4WCz2idB28NPOidga
동해물과 백두산이 마르고 닳도록
```

* **설명**  
```
문자열 s를 Base64String, AES128, AES256 으로 암복호화 
```
<br />

#### 7-4 LauncherTool  
중복 실행 방지, 지연 실행

* **샘플코드**
```csharp
static void Main(string[] args)
{
    Console.WriteLine($"시작 시간:{DateTime.Now.ToString()}");
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        LauncherTool.Start("Sample", () =>
        {
            Console.WriteLine($"정상 실행:{DateTime.Now.ToString()}");
        }, () =>
        {
            Console.WriteLine($"중복 실행:{DateTime.Now.ToString()}");
        }, true, 1000);

        Console.ReadKey();
    }
}
```

* **결과**  
```
시작 시간:2022-11-08 오후 2:38:31
정상 실행:2022-11-08 오후 2:38:32
```
```
시작 시간:2022-11-08 오후 2:38:42
중복 실행:2022-11-08 오후 2:38:42
```

* **설명**  
```
정상 실행시 코드와 중복 실행시 코드 분리
1초 지연실행 
```
<br />

#### 7-5 MathTool  
수학 관련 함수 

* **샘플코드**
```csharp
static void Main(string[] args)
{
    Console.WriteLine(MathTool.Center(100, 150));
    Console.WriteLine(MathTool.CenterDist(100, 150));
    Console.WriteLine(MathTool.CenterPoint(new Rectangle(100, 100, 200, 150)));
    Console.WriteLine(MathTool.CompareAngle(900, 150, 200));
    Console.WriteLine(MathTool.Constrain(120, 0, 100));
    Console.WriteLine(MathTool.GetAngle(new Point(0, 0), new Point(30, 60)));
    Console.WriteLine(MathTool.GetDistance(new Point(0, 0), new Point(30, 60)));
    Console.WriteLine(MathTool.GetPointWithAngle(new Point(0, 0), 45, 50));
    Console.WriteLine(MathTool.LinearEquationX(new Point(0, 0), new Point(30, 20), 10));
    Console.WriteLine(MathTool.LinearEquationY(new Point(0, 0), new Point(30, 20), 15));
    Console.WriteLine(MathTool.Map(10, 0, 100, 20, 50));
    Console.WriteLine(MathTool.RotatePoint(new Point(0, 0), new Point(30, 30), 45));

    Console.ReadKey();
}
```

* **결과 및 설명**  
```
125                         : 100 부터 150 사이 중간값 
175                         : 100 부터 150 거리 사이 중간값
{X=200,Y=175}               : 사각형(100,100,200,150)의 중심점
True                        : 900도는 150도와 200도 사이 있는가?
100                         : 120을 0~100 범위로 제한
63.43494882292201           : Point(0,0)과 Point(30,60) 사이 각 
67.08203932499369           : Point(0,0)과 Point(30,60) 사이 거리
{X=35.35534, Y=35.35534}    : Point(0,0)에서 45도 50 거리 위치한 지점 
15                          : Point(0,0)과 Point(30,20)을 지나는 직선상 Y좌표가 10일 때 X의 값
10                          : Point(0,0)과 Point(30,20)을 지나는 직선상 X좌표가 15일 때 Y의 값
23                          : 0~100 사이 값 10은 20~50 일 때
{X=0, Y=42.426407}          : Point(0,0)을 기준으로 Point(30,30)을 45도 회전 시 위치
```
<br />

### 8 Devinno.Utils
#### 8-1 ExternalProgram  
수학 관련 함수 

* **샘플코드**
```csharp
static void Main(string[] args)
{
    var v = new ExternalProgram("calc.exe");
    v.Start();
    Thread.Sleep(5000);
    v.Stop();

    Console.ReadKey();
}
```

* **설명**  
```
계산기 프로그램 실행 5초 후 종료
```
<br />

