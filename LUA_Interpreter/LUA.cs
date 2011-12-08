// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2008
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.5.190
// Machine:  AVATAR-��
// DateTime: 08.12.2011 14:38:22
// UserName: Avatar
// Input file <Grammar_LUA.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using LUA_Interpreter;

namespace LuaSyntax
{
public enum Tokens {error=126,
    EOF=127,str=128,digit=129,identifer=130,end=131,inT=132,
    repeatT=133,breakT=134,falseT=135,local=136,returnT=137,doT=138,
    forT=139,nil=140,then=141,elseT=142,functionT=143,trueT=144,
    elseifT=145,ifT=146,untilT=147,whileT=148,SMTH=149,ge=150,
    le=151,e=152,ne=153,doublePoint=154,or=155,and=156,
    not=157,UMINUS=158};

public struct ValueType
{
	public string s;	
	public int i;
	public double d;
}
// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
	IdentiferTable m_table;
	double _baseNumber = 0;
#pragma warning disable 649
    private Dictionary<int, string> aliasses;
#pragma warning restore 649

  protected override void Initialize()
  {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);

    this.InitStateTable(217);
    AddState(0,new State(-4,new int[]{-16,1,-18,3}));
    AddState(1,new State(new int[]{127,2}));
    AddState(2,new State(-1));
    AddState(3,new State(new int[]{127,4,130,52,138,73,148,78,133,126,146,132,137,147,134,150,139,151,143,174,136,196},new int[]{-6,5,-3,8,-5,67,-1,51,-40,53,-21,72}));
    AddState(4,new State(-2));
    AddState(5,new State(new int[]{59,7,127,-6,130,-6,138,-6,148,-6,133,-6,146,-6,137,-6,134,-6,139,-6,143,-6,136,-6,131,-6,147,-6,145,-6,142,-6},new int[]{-19,6}));
    AddState(6,new State(-3));
    AddState(7,new State(-5));
    AddState(8,new State(new int[]{61,9}));
    AddState(9,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-4,10,-41,149,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(10,new State(new int[]{44,11,59,-8,127,-8,130,-8,138,-8,148,-8,133,-8,146,-8,137,-8,134,-8,139,-8,143,-8,136,-8,131,-8,147,-8,145,-8,142,-8}));
    AddState(11,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,12,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(12,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-61,59,-61,127,-61,130,-61,138,-61,148,-61,133,-61,146,-61,137,-61,134,-61,139,-61,143,-61,136,-61,131,-61,147,-61,145,-61,142,-61,41,-61}));
    AddState(13,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,14,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(14,new State(new int[]{42,-72,47,-72,37,-72,43,-72,45,-72,94,23,154,-72,60,-72,62,-72,151,-72,150,-72,152,-72,153,-72,156,-72,155,-72,44,-72,59,-72,127,-72,130,-72,138,-72,148,-72,133,-72,146,-72,137,-72,134,-72,139,-72,143,-72,136,-72,131,-72,147,-72,145,-72,142,-72,41,-72,93,-72,125,-72,141,-72}));
    AddState(15,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,16,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(16,new State(new int[]{42,-73,47,-73,37,-73,43,-73,45,-73,94,23,154,-73,60,-73,62,-73,151,-73,150,-73,152,-73,153,-73,156,-73,155,-73,44,-73,59,-73,127,-73,130,-73,138,-73,148,-73,133,-73,146,-73,137,-73,134,-73,139,-73,143,-73,136,-73,131,-73,147,-73,145,-73,142,-73,41,-73,93,-73,125,-73,141,-73}));
    AddState(17,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,18,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(18,new State(new int[]{42,-74,47,-74,37,-74,43,-74,45,-74,94,23,154,-74,60,-74,62,-74,151,-74,150,-74,152,-74,153,-74,156,-74,155,-74,44,-74,59,-74,127,-74,130,-74,138,-74,148,-74,133,-74,146,-74,137,-74,134,-74,139,-74,143,-74,136,-74,131,-74,147,-74,145,-74,142,-74,41,-74,93,-74,125,-74,141,-74}));
    AddState(19,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,20,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(20,new State(new int[]{42,13,47,15,37,17,43,-75,45,-75,94,23,154,-75,60,-75,62,-75,151,-75,150,-75,152,-75,153,-75,156,39,155,-75,44,-75,59,-75,127,-75,130,-75,138,-75,148,-75,133,-75,146,-75,137,-75,134,-75,139,-75,143,-75,136,-75,131,-75,147,-75,145,-75,142,-75,41,-75,93,-75,125,-75,141,-75}));
    AddState(21,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,22,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(22,new State(new int[]{42,13,47,15,37,17,43,-76,45,-76,94,23,154,-76,60,-76,62,-76,151,-76,150,-76,152,-76,153,-76,156,39,155,-76,44,-76,59,-76,127,-76,130,-76,138,-76,148,-76,133,-76,146,-76,137,-76,134,-76,139,-76,143,-76,136,-76,131,-76,147,-76,145,-76,142,-76,41,-76,93,-76,125,-76,141,-76}));
    AddState(23,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,24,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(24,new State(-77));
    AddState(25,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,26,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(26,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,-78,60,-78,62,-78,151,-78,150,-78,152,-78,153,-78,156,39,155,41,44,-78,59,-78,127,-78,130,-78,138,-78,148,-78,133,-78,146,-78,137,-78,134,-78,139,-78,143,-78,136,-78,131,-78,147,-78,145,-78,142,-78,41,-78,93,-78,125,-78,141,-78}));
    AddState(27,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,28,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(28,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-79,62,-79,151,-79,150,-79,152,-79,153,-79,156,39,155,41,44,-79,59,-79,127,-79,130,-79,138,-79,148,-79,133,-79,146,-79,137,-79,134,-79,139,-79,143,-79,136,-79,131,-79,147,-79,145,-79,142,-79,41,-79,93,-79,125,-79,141,-79}));
    AddState(29,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,30,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(30,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-80,62,-80,151,-80,150,-80,152,-80,153,-80,156,39,155,41,44,-80,59,-80,127,-80,130,-80,138,-80,148,-80,133,-80,146,-80,137,-80,134,-80,139,-80,143,-80,136,-80,131,-80,147,-80,145,-80,142,-80,41,-80,93,-80,125,-80,141,-80}));
    AddState(31,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,32,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(32,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-81,62,-81,151,-81,150,-81,152,-81,153,-81,156,39,155,41,44,-81,59,-81,127,-81,130,-81,138,-81,148,-81,133,-81,146,-81,137,-81,134,-81,139,-81,143,-81,136,-81,131,-81,147,-81,145,-81,142,-81,41,-81,93,-81,125,-81,141,-81}));
    AddState(33,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,34,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(34,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-82,62,-82,151,-82,150,-82,152,-82,153,-82,156,39,155,41,44,-82,59,-82,127,-82,130,-82,138,-82,148,-82,133,-82,146,-82,137,-82,134,-82,139,-82,143,-82,136,-82,131,-82,147,-82,145,-82,142,-82,41,-82,93,-82,125,-82,141,-82}));
    AddState(35,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,36,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(36,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-83,62,-83,151,-83,150,-83,152,-83,153,-83,156,39,155,41,44,-83,59,-83,127,-83,130,-83,138,-83,148,-83,133,-83,146,-83,137,-83,134,-83,139,-83,143,-83,136,-83,131,-83,147,-83,145,-83,142,-83,41,-83,93,-83,125,-83,141,-83}));
    AddState(37,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,38,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(38,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,-84,62,-84,151,-84,150,-84,152,-84,153,-84,156,39,155,41,44,-84,59,-84,127,-84,130,-84,138,-84,148,-84,133,-84,146,-84,137,-84,134,-84,139,-84,143,-84,136,-84,131,-84,147,-84,145,-84,142,-84,41,-84,93,-84,125,-84,141,-84}));
    AddState(39,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,40,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(40,new State(new int[]{42,-85,47,-85,37,-85,43,-85,45,-85,94,23,154,-85,60,-85,62,-85,151,-85,150,-85,152,-85,153,-85,156,-85,155,-85,44,-85,59,-85,127,-85,130,-85,138,-85,148,-85,133,-85,146,-85,137,-85,134,-85,139,-85,143,-85,136,-85,131,-85,147,-85,145,-85,142,-85,41,-85,93,-85,125,-85,141,-85}));
    AddState(41,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,42,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(42,new State(new int[]{42,13,47,15,37,17,43,-86,45,-86,94,23,154,-86,60,-86,62,-86,151,-86,150,-86,152,-86,153,-86,156,39,155,-86,44,-86,59,-86,127,-86,130,-86,138,-86,148,-86,133,-86,146,-86,137,-86,134,-86,139,-86,143,-86,136,-86,131,-86,147,-86,145,-86,142,-86,41,-86,93,-86,125,-86,141,-86}));
    AddState(43,new State(-63));
    AddState(44,new State(new int[]{129,45,46,46,42,-64,47,-64,37,-64,43,-64,45,-64,94,-64,154,-64,60,-64,62,-64,151,-64,150,-64,152,-64,153,-64,156,-64,155,-64,44,-64,59,-64,127,-64,130,-64,138,-64,148,-64,133,-64,146,-64,137,-64,134,-64,139,-64,143,-64,136,-64,131,-64,147,-64,145,-64,142,-64,41,-64,93,-64,125,-64,141,-64}));
    AddState(45,new State(-123));
    AddState(46,new State(new int[]{129,47}));
    AddState(47,new State(-124));
    AddState(48,new State(-125));
    AddState(49,new State(-65));
    AddState(50,new State(new int[]{42,-66,47,-66,37,-66,43,-66,45,-66,94,-66,154,-66,60,-66,62,-66,151,-66,150,-66,152,-66,153,-66,156,-66,155,-66,44,-66,59,-66,127,-66,130,-66,138,-66,148,-66,133,-66,146,-66,137,-66,134,-66,139,-66,143,-66,136,-66,131,-66,147,-66,145,-66,142,-66,41,-66,93,-66,125,-66,141,-66,91,-54,46,-54,58,-54,40,-54,123,-54,128,-54}));
    AddState(51,new State(-51));
    AddState(52,new State(-122));
    AddState(53,new State(new int[]{91,54,46,205,58,208,40,211,123,89,128,216},new int[]{-45,207,-44,215}));
    AddState(54,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,55,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(55,new State(new int[]{93,56}));
    AddState(56,new State(-52));
    AddState(57,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,93,-62,138,-62,59,-62,127,-62,130,-62,148,-62,133,-62,146,-62,137,-62,134,-62,139,-62,143,-62,136,-62,131,-62,147,-62,145,-62,142,-62,141,-62,44,-62}));
    AddState(58,new State(new int[]{91,-55,46,-55,58,-55,40,-55,123,-55,128,-55,42,-69,47,-69,37,-69,43,-69,45,-69,94,-69,154,-69,60,-69,62,-69,151,-69,150,-69,152,-69,153,-69,156,-69,155,-69,44,-69,59,-69,127,-69,130,-69,138,-69,148,-69,133,-69,146,-69,137,-69,134,-69,139,-69,143,-69,136,-69,131,-69,147,-69,145,-69,142,-69,41,-69,93,-69,125,-69,141,-69}));
    AddState(59,new State(-67));
    AddState(60,new State(new int[]{40,61}));
    AddState(61,new State(new int[]{149,184,130,52,41,-38},new int[]{-12,62,-13,183,-1,185}));
    AddState(62,new State(new int[]{41,63}));
    AddState(63,new State(-4,new int[]{-20,64,-18,66}));
    AddState(64,new State(new int[]{131,65}));
    AddState(65,new State(-97));
    AddState(66,new State(new int[]{130,52,138,73,148,78,133,126,146,132,137,147,134,150,139,151,143,174,136,196,131,-7,147,-7,145,-7,142,-7},new int[]{-6,5,-3,8,-5,67,-1,51,-40,53,-21,72}));
    AddState(67,new State(new int[]{91,-54,46,-54,58,-54,40,-54,123,-54,128,-54,44,-50,61,-50},new int[]{-10,68}));
    AddState(68,new State(new int[]{44,69,61,-48}));
    AddState(69,new State(new int[]{130,52},new int[]{-5,70,-1,51,-40,53,-21,71}));
    AddState(70,new State(new int[]{44,-49,61,-49,91,-54,46,-54,58,-54,40,-54,123,-54,128,-54}));
    AddState(71,new State(-55));
    AddState(72,new State(new int[]{91,-55,46,-55,58,-55,40,-55,123,-55,128,-55,59,-9,127,-9,130,-9,138,-9,148,-9,133,-9,146,-9,137,-9,134,-9,139,-9,143,-9,136,-9,131,-9,147,-9,145,-9,142,-9}));
    AddState(73,new State(-10,new int[]{-22,74}));
    AddState(74,new State(-4,new int[]{-20,75,-18,66}));
    AddState(75,new State(-11,new int[]{-23,76}));
    AddState(76,new State(new int[]{131,77}));
    AddState(77,new State(-12));
    AddState(78,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,79,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(79,new State(new int[]{138,80}));
    AddState(80,new State(-13,new int[]{-25,81}));
    AddState(81,new State(-4,new int[]{-20,82,-18,66}));
    AddState(82,new State(-14,new int[]{-26,83}));
    AddState(83,new State(new int[]{131,84}));
    AddState(84,new State(-15));
    AddState(85,new State(-68));
    AddState(86,new State(new int[]{130,52},new int[]{-1,87}));
    AddState(87,new State(-102));
    AddState(88,new State(-70));
    AddState(89,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112,91,100,59,-109,125,-109},new int[]{-47,90,-48,92,-50,117,-41,118,-2,44,-5,50,-1,122,-40,53,-21,58,-42,59,-43,85,-44,88,-49,123,-51,95,-53,96}));
    AddState(90,new State(new int[]{125,91}));
    AddState(91,new State(-103));
    AddState(92,new State(new int[]{59,93,125,-104}));
    AddState(93,new State(new int[]{91,100,130,52,125,-111},new int[]{-49,94,-51,95,-53,96,-1,114}));
    AddState(94,new State(-106));
    AddState(95,new State(-110));
    AddState(96,new State(-119,new int[]{-54,97}));
    AddState(97,new State(new int[]{44,98,59,-115,125,-115}));
    AddState(98,new State(new int[]{91,100,130,52,59,-114,125,-114},new int[]{-53,99,-1,114}));
    AddState(99,new State(-118));
    AddState(100,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,101,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(101,new State(new int[]{93,102,42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41}));
    AddState(102,new State(new int[]{61,103}));
    AddState(103,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,104,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(104,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-120,59,-120,125,-120}));
    AddState(105,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,106,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(106,new State(new int[]{41,107,42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41}));
    AddState(107,new State(-71));
    AddState(108,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,109,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(109,new State(-87));
    AddState(110,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,111,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(111,new State(-88));
    AddState(112,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,113,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(113,new State(-89));
    AddState(114,new State(new int[]{61,115}));
    AddState(115,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-41,116,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(116,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-121,59,-121,125,-121}));
    AddState(117,new State(-108));
    AddState(118,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-117,59,-117,125,-117},new int[]{-52,119}));
    AddState(119,new State(new int[]{44,120,59,-113,125,-113}));
    AddState(120,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112,59,-112,125,-112},new int[]{-41,121,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(121,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-116,59,-116,125,-116}));
    AddState(122,new State(new int[]{61,115,42,-51,47,-51,37,-51,43,-51,45,-51,94,-51,154,-51,60,-51,62,-51,151,-51,150,-51,152,-51,153,-51,156,-51,155,-51,44,-51,59,-51,125,-51,91,-51,46,-51,58,-51,40,-51,123,-51,128,-51}));
    AddState(123,new State(new int[]{59,124,125,-105}));
    AddState(124,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112,125,-109},new int[]{-48,125,-50,117,-41,118,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(125,new State(-107));
    AddState(126,new State(-16,new int[]{-27,127}));
    AddState(127,new State(-4,new int[]{-20,128,-18,66}));
    AddState(128,new State(-17,new int[]{-28,129}));
    AddState(129,new State(new int[]{147,130}));
    AddState(130,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,131,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(131,new State(-18));
    AddState(132,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,133,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(133,new State(new int[]{141,134}));
    AddState(134,new State(-19,new int[]{-29,135}));
    AddState(135,new State(-4,new int[]{-20,136,-18,66}));
    AddState(136,new State(-20,new int[]{-30,137}));
    AddState(137,new State(-42,new int[]{-31,138}));
    AddState(138,new State(new int[]{145,141,142,145,131,-44},new int[]{-32,139}));
    AddState(139,new State(new int[]{131,140}));
    AddState(140,new State(-21));
    AddState(141,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,142,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(142,new State(new int[]{141,143}));
    AddState(143,new State(-4,new int[]{-20,144,-18,66}));
    AddState(144,new State(-41));
    AddState(145,new State(-4,new int[]{-20,146,-18,66}));
    AddState(146,new State(-43));
    AddState(147,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112,59,-22,127,-22,138,-22,148,-22,133,-22,146,-22,137,-22,134,-22,139,-22,136,-22,131,-22,147,-22,145,-22,142,-22},new int[]{-4,148,-41,149,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(148,new State(new int[]{44,11,59,-23,127,-23,130,-23,138,-23,148,-23,133,-23,146,-23,137,-23,134,-23,139,-23,143,-23,136,-23,131,-23,147,-23,145,-23,142,-23}));
    AddState(149,new State(new int[]{42,13,47,15,37,17,43,19,45,21,94,23,154,25,60,27,62,29,151,31,150,33,152,35,153,37,156,39,155,41,44,-60,59,-60,127,-60,130,-60,138,-60,148,-60,133,-60,146,-60,137,-60,134,-60,139,-60,143,-60,136,-60,131,-60,147,-60,145,-60,142,-60,41,-60}));
    AddState(150,new State(-24));
    AddState(151,new State(new int[]{130,52},new int[]{-1,152}));
    AddState(152,new State(new int[]{61,153,44,165}));
    AddState(153,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,154,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(154,new State(new int[]{44,155}));
    AddState(155,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,156,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(156,new State(new int[]{44,163,138,-36},new int[]{-33,157}));
    AddState(157,new State(new int[]{138,158}));
    AddState(158,new State(-25,new int[]{-34,159}));
    AddState(159,new State(-4,new int[]{-20,160,-18,66}));
    AddState(160,new State(-26,new int[]{-35,161}));
    AddState(161,new State(new int[]{131,162}));
    AddState(162,new State(-27));
    AddState(163,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,164,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(164,new State(-35));
    AddState(165,new State(new int[]{130,52},new int[]{-1,166}));
    AddState(166,new State(new int[]{132,167}));
    AddState(167,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-24,168,-41,57,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(168,new State(new int[]{138,169}));
    AddState(169,new State(-28,new int[]{-36,170}));
    AddState(170,new State(-4,new int[]{-20,171,-18,66}));
    AddState(171,new State(-29,new int[]{-37,172}));
    AddState(172,new State(new int[]{131,173}));
    AddState(173,new State(-30));
    AddState(174,new State(new int[]{130,52},new int[]{-11,175,-1,191}));
    AddState(175,new State(new int[]{40,176}));
    AddState(176,new State(new int[]{149,184,130,52,41,-38},new int[]{-12,177,-13,183,-1,185}));
    AddState(177,new State(new int[]{41,178}));
    AddState(178,new State(-31,new int[]{-38,179}));
    AddState(179,new State(-4,new int[]{-20,180,-18,66}));
    AddState(180,new State(-32,new int[]{-39,181}));
    AddState(181,new State(new int[]{131,182}));
    AddState(182,new State(-33));
    AddState(183,new State(-37));
    AddState(184,new State(-98));
    AddState(185,new State(-58,new int[]{-15,186}));
    AddState(186,new State(new int[]{44,188,41,-101},new int[]{-14,187}));
    AddState(187,new State(-99));
    AddState(188,new State(new int[]{149,190,130,52},new int[]{-1,189}));
    AddState(189,new State(-57));
    AddState(190,new State(-100));
    AddState(191,new State(new int[]{46,192,58,194,40,-45}));
    AddState(192,new State(new int[]{130,52},new int[]{-1,193}));
    AddState(193,new State(-46));
    AddState(194,new State(new int[]{130,52},new int[]{-1,195}));
    AddState(195,new State(-47));
    AddState(196,new State(new int[]{130,52},new int[]{-7,197,-1,202}));
    AddState(197,new State(new int[]{61,200,59,-40,127,-40,130,-40,138,-40,148,-40,133,-40,146,-40,137,-40,134,-40,139,-40,143,-40,136,-40,131,-40,147,-40,145,-40,142,-40},new int[]{-8,198,-9,199}));
    AddState(198,new State(-34));
    AddState(199,new State(-39));
    AddState(200,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112},new int[]{-4,201,-41,149,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(201,new State(new int[]{44,11,59,-59,127,-59,130,-59,138,-59,148,-59,133,-59,146,-59,137,-59,134,-59,139,-59,143,-59,136,-59,131,-59,147,-59,145,-59,142,-59}));
    AddState(202,new State(-58,new int[]{-15,203}));
    AddState(203,new State(new int[]{44,204,61,-56,59,-56,127,-56,130,-56,138,-56,148,-56,133,-56,146,-56,137,-56,134,-56,139,-56,143,-56,136,-56,131,-56,147,-56,145,-56,142,-56}));
    AddState(204,new State(new int[]{130,52},new int[]{-1,189}));
    AddState(205,new State(new int[]{130,52},new int[]{-1,206}));
    AddState(206,new State(-53));
    AddState(207,new State(-90));
    AddState(208,new State(new int[]{130,52},new int[]{-1,209}));
    AddState(209,new State(new int[]{40,211,123,89,128,216},new int[]{-45,210,-44,215}));
    AddState(210,new State(-91));
    AddState(211,new State(new int[]{140,43,129,48,128,49,130,52,143,60,37,86,123,89,40,105,157,108,35,110,158,112,41,-96},new int[]{-46,212,-4,214,-41,149,-2,44,-5,50,-1,51,-40,53,-21,58,-42,59,-43,85,-44,88}));
    AddState(212,new State(new int[]{41,213}));
    AddState(213,new State(-92));
    AddState(214,new State(new int[]{44,11,41,-95}));
    AddState(215,new State(-93));
    AddState(216,new State(-94));

    Rule[] rules=new Rule[126];
    rules[1]=new Rule(-17, new int[]{-16,127});
    rules[2]=new Rule(-16, new int[]{-18,127});
    rules[3]=new Rule(-18, new int[]{-18,-6,-19});
    rules[4]=new Rule(-18, new int[]{});
    rules[5]=new Rule(-19, new int[]{59});
    rules[6]=new Rule(-19, new int[]{});
    rules[7]=new Rule(-20, new int[]{-18});
    rules[8]=new Rule(-6, new int[]{-3,61,-4});
    rules[9]=new Rule(-6, new int[]{-21});
    rules[10]=new Rule(-22, new int[]{});
    rules[11]=new Rule(-23, new int[]{});
    rules[12]=new Rule(-6, new int[]{138,-22,-20,-23,131});
    rules[13]=new Rule(-25, new int[]{});
    rules[14]=new Rule(-26, new int[]{});
    rules[15]=new Rule(-6, new int[]{148,-24,138,-25,-20,-26,131});
    rules[16]=new Rule(-27, new int[]{});
    rules[17]=new Rule(-28, new int[]{});
    rules[18]=new Rule(-6, new int[]{133,-27,-20,-28,147,-24});
    rules[19]=new Rule(-29, new int[]{});
    rules[20]=new Rule(-30, new int[]{});
    rules[21]=new Rule(-6, new int[]{146,-24,141,-29,-20,-30,-31,-32,131});
    rules[22]=new Rule(-6, new int[]{137});
    rules[23]=new Rule(-6, new int[]{137,-4});
    rules[24]=new Rule(-6, new int[]{134});
    rules[25]=new Rule(-34, new int[]{});
    rules[26]=new Rule(-35, new int[]{});
    rules[27]=new Rule(-6, new int[]{139,-1,61,-24,44,-24,-33,138,-34,-20,-35,131});
    rules[28]=new Rule(-36, new int[]{});
    rules[29]=new Rule(-37, new int[]{});
    rules[30]=new Rule(-6, new int[]{139,-1,44,-1,132,-24,138,-36,-20,-37,131});
    rules[31]=new Rule(-38, new int[]{});
    rules[32]=new Rule(-39, new int[]{});
    rules[33]=new Rule(-6, new int[]{143,-11,40,-12,41,-38,-20,-39,131});
    rules[34]=new Rule(-6, new int[]{136,-7,-8});
    rules[35]=new Rule(-33, new int[]{44,-24});
    rules[36]=new Rule(-33, new int[]{});
    rules[37]=new Rule(-12, new int[]{-13});
    rules[38]=new Rule(-12, new int[]{});
    rules[39]=new Rule(-8, new int[]{-9});
    rules[40]=new Rule(-8, new int[]{});
    rules[41]=new Rule(-31, new int[]{-31,145,-24,141,-20});
    rules[42]=new Rule(-31, new int[]{});
    rules[43]=new Rule(-32, new int[]{142,-20});
    rules[44]=new Rule(-32, new int[]{});
    rules[45]=new Rule(-11, new int[]{-1});
    rules[46]=new Rule(-11, new int[]{-1,46,-1});
    rules[47]=new Rule(-11, new int[]{-1,58,-1});
    rules[48]=new Rule(-3, new int[]{-5,-10});
    rules[49]=new Rule(-10, new int[]{-10,44,-5});
    rules[50]=new Rule(-10, new int[]{});
    rules[51]=new Rule(-5, new int[]{-1});
    rules[52]=new Rule(-5, new int[]{-40,91,-24,93});
    rules[53]=new Rule(-5, new int[]{-40,46,-1});
    rules[54]=new Rule(-40, new int[]{-5});
    rules[55]=new Rule(-40, new int[]{-21});
    rules[56]=new Rule(-7, new int[]{-1,-15});
    rules[57]=new Rule(-15, new int[]{-15,44,-1});
    rules[58]=new Rule(-15, new int[]{});
    rules[59]=new Rule(-9, new int[]{61,-4});
    rules[60]=new Rule(-4, new int[]{-41});
    rules[61]=new Rule(-4, new int[]{-4,44,-41});
    rules[62]=new Rule(-24, new int[]{-41});
    rules[63]=new Rule(-41, new int[]{140});
    rules[64]=new Rule(-41, new int[]{-2});
    rules[65]=new Rule(-41, new int[]{128});
    rules[66]=new Rule(-41, new int[]{-5});
    rules[67]=new Rule(-41, new int[]{-42});
    rules[68]=new Rule(-41, new int[]{-43});
    rules[69]=new Rule(-41, new int[]{-21});
    rules[70]=new Rule(-41, new int[]{-44});
    rules[71]=new Rule(-41, new int[]{40,-41,41});
    rules[72]=new Rule(-41, new int[]{-41,42,-41});
    rules[73]=new Rule(-41, new int[]{-41,47,-41});
    rules[74]=new Rule(-41, new int[]{-41,37,-41});
    rules[75]=new Rule(-41, new int[]{-41,43,-41});
    rules[76]=new Rule(-41, new int[]{-41,45,-41});
    rules[77]=new Rule(-41, new int[]{-41,94,-41});
    rules[78]=new Rule(-41, new int[]{-41,154,-41});
    rules[79]=new Rule(-41, new int[]{-41,60,-41});
    rules[80]=new Rule(-41, new int[]{-41,62,-41});
    rules[81]=new Rule(-41, new int[]{-41,151,-41});
    rules[82]=new Rule(-41, new int[]{-41,150,-41});
    rules[83]=new Rule(-41, new int[]{-41,152,-41});
    rules[84]=new Rule(-41, new int[]{-41,153,-41});
    rules[85]=new Rule(-41, new int[]{-41,156,-41});
    rules[86]=new Rule(-41, new int[]{-41,155,-41});
    rules[87]=new Rule(-41, new int[]{157,-41});
    rules[88]=new Rule(-41, new int[]{35,-41});
    rules[89]=new Rule(-41, new int[]{158,-41});
    rules[90]=new Rule(-21, new int[]{-40,-45});
    rules[91]=new Rule(-21, new int[]{-40,58,-1,-45});
    rules[92]=new Rule(-45, new int[]{40,-46,41});
    rules[93]=new Rule(-45, new int[]{-44});
    rules[94]=new Rule(-45, new int[]{128});
    rules[95]=new Rule(-46, new int[]{-4});
    rules[96]=new Rule(-46, new int[]{});
    rules[97]=new Rule(-42, new int[]{143,40,-12,41,-20,131});
    rules[98]=new Rule(-13, new int[]{149});
    rules[99]=new Rule(-13, new int[]{-1,-15,-14});
    rules[100]=new Rule(-14, new int[]{44,149});
    rules[101]=new Rule(-14, new int[]{});
    rules[102]=new Rule(-43, new int[]{37,-1});
    rules[103]=new Rule(-44, new int[]{123,-47,125});
    rules[104]=new Rule(-47, new int[]{-48});
    rules[105]=new Rule(-47, new int[]{-49});
    rules[106]=new Rule(-47, new int[]{-48,59,-49});
    rules[107]=new Rule(-47, new int[]{-49,59,-48});
    rules[108]=new Rule(-48, new int[]{-50});
    rules[109]=new Rule(-48, new int[]{});
    rules[110]=new Rule(-49, new int[]{-51});
    rules[111]=new Rule(-49, new int[]{});
    rules[112]=new Rule(-50, new int[]{-41,-52,44});
    rules[113]=new Rule(-50, new int[]{-41,-52});
    rules[114]=new Rule(-51, new int[]{-53,-54,44});
    rules[115]=new Rule(-51, new int[]{-53,-54});
    rules[116]=new Rule(-52, new int[]{-52,44,-41});
    rules[117]=new Rule(-52, new int[]{});
    rules[118]=new Rule(-54, new int[]{-54,44,-53});
    rules[119]=new Rule(-54, new int[]{});
    rules[120]=new Rule(-53, new int[]{91,-41,93,61,-41});
    rules[121]=new Rule(-53, new int[]{-1,61,-41});
    rules[122]=new Rule(-1, new int[]{130});
    rules[123]=new Rule(-2, new int[]{-2,129});
    rules[124]=new Rule(-2, new int[]{-2,46,129});
    rules[125]=new Rule(-2, new int[]{129});
    this.InitRules(rules);

    this.InitNonTerminals(new string[] {"", "NAME", "NUMBER", "VARLIST1", "EXPLIST1", 
      "VAR", "STAT", "DECLIST", "INIT_OPT", "INIT", "VAR_LIST", "FUNCNAME", "PL_OPT", 
      "PARLIST1", "SMTH_OPT", "NAME_LIST", "PROGRAM", "$accept", "CHUNK", "COMMA", 
      "BLOCK", "FUNCTION_CALL", "Anon@1", "Anon@2", "EXP1", "Anon@3", "Anon@4", 
      "Anon@5", "Anon@6", "Anon@7", "Anon@8", "ELSEIF_LIST", "ELSE_LIST", "EXPTEMP", 
      "Anon@9", "Anon@10", "Anon@11", "Anon@12", "Anon@13", "Anon@14", "VARORFUNC", 
      "EXP", "FUNCTION", "UPVALUE", "TABLE_CONSTRUCTOR", "ARGS", "EXP_LIST_OPT", 
      "FIELDLIST", "LFIELDLIST", "FFIELDLIST", "LFIELDLIST1", "FFIELDLIST1", 
      "LST", "FFIELD", "FST", });
  }

  protected override void DoAction(int action)
  {
    switch (action)
    {
      case 8: // STAT -> VARLIST1, '=', EXPLIST1
{ m_table.AddIdentiferList(ValueStack[ValueStack.Depth-3].s, ValueStack[ValueStack.Depth-1].s); }
        break;
      case 10: // Anon@1 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 11: // Anon@2 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 13: // Anon@3 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 14: // Anon@4 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 16: // Anon@5 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 17: // Anon@6 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 19: // Anon@7 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 20: // Anon@8 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 25: // Anon@9 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 26: // Anon@10 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 28: // Anon@11 -> /* empty */
{m_table.AddBlockStartMark();}
        break;
      case 29: // Anon@12 -> /* empty */
{m_table.AddBlockEndMark();}
        break;
      case 31: // Anon@13 -> /* empty */
{ m_table.AddFunctionDefinition(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].s); }
        break;
      case 32: // Anon@14 -> /* empty */
{ m_table.ReturnFromFunction() ;}
        break;
      case 34: // STAT -> local, DECLIST, INIT_OPT
{m_table.AddIdentiferList(ValueStack[ValueStack.Depth-2].s, ValueStack[ValueStack.Depth-1].s, false);}
        break;
      case 37: // PL_OPT -> PARLIST1
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s;}
        break;
      case 38: // PL_OPT -> /* empty */
{CurrentSemanticValue.s = "";}
        break;
      case 39: // INIT_OPT -> INIT
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s;}
        break;
      case 45: // FUNCNAME -> NAME
{CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s;}
        break;
      case 48: // VARLIST1 -> VAR, VAR_LIST
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-2].s + ' ' + ValueStack[ValueStack.Depth-1].s;}
        break;
      case 49: // VAR_LIST -> VAR_LIST, ',', VAR
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + ' ' + ValueStack[ValueStack.Depth-1].s;}
        break;
      case 50: // VAR_LIST -> /* empty */
{CurrentSemanticValue.s = "";}
        break;
      case 51: // VAR -> NAME
{CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s;}
        break;
      case 56: // DECLIST -> NAME, NAME_LIST
{CurrentSemanticValue.s = ValueStack[ValueStack.Depth-2].s + ' ' + ValueStack[ValueStack.Depth-1].s;}
        break;
      case 57: // NAME_LIST -> NAME_LIST, ',', NAME
{CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + ' ' + ValueStack[ValueStack.Depth-1].s;}
        break;
      case 58: // NAME_LIST -> /* empty */
{CurrentSemanticValue.s = "";}
        break;
      case 64: // EXP -> NUMBER
{Console.Error.WriteLine("Number semantics: {0}", ValueStack[ValueStack.Depth-1].d);}
        break;
      case 66: // EXP -> VAR
{m_table.CheckIdentiferVisibility(ValueStack[ValueStack.Depth-1].s);}
        break;
      case 98: // PARLIST1 -> SMTH
{CurrentSemanticValue.s = "...";}
        break;
      case 99: // PARLIST1 -> NAME, NAME_LIST, SMTH_OPT
{CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + ' ' + ValueStack[ValueStack.Depth-2].s + ' ' + ValueStack[ValueStack.Depth-1].s;}
        break;
      case 100: // SMTH_OPT -> ',', SMTH
{CurrentSemanticValue.s = "...";}
        break;
      case 101: // SMTH_OPT -> /* empty */
{CurrentSemanticValue.s = "";}
        break;
      case 122: // NAME -> identifer
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 123: // NUMBER -> NUMBER, digit
{ if (_baseNumber == 0) {CurrentSemanticValue.d = ValueStack[ValueStack.Depth-2].d * 10 + ValueStack[ValueStack.Depth-1].i;} else {_baseNumber*=0.1; CurrentSemanticValue.d = ValueStack[ValueStack.Depth-2].d + ValueStack[ValueStack.Depth-1].i * _baseNumber; } }
        break;
      case 124: // NUMBER -> NUMBER, '.', digit
{ if (_baseNumber != 0) {YYError();} else {_baseNumber = 0.1;CurrentSemanticValue.d = ValueStack[ValueStack.Depth-3].d + ValueStack[ValueStack.Depth-1].i * _baseNumber;}}
        break;
      case 125: // NUMBER -> digit
{ CurrentSemanticValue.d = ValueStack[ValueStack.Depth-1].i; _baseNumber = 0; }
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }


public Parser(IdentiferTable table) : base(null) { m_table = table; }
}
}
