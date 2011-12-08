//#define TRACE_ACTIONS
#define USING_DEBUG_NAMES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUA_Interpreter
{
    public class IdentiferTable
    {
        public const int UNDEF = 0;//or nil
        public const int STRING = 1;
        public const int NUMBER = 2;
        public const int FUNCTION = 3;
        public const int BOOLEAN = 4;
        public const int TABLE = 5;
        public const int BLOCK_MARK_BEGIN = 6;
        public const int BLOCK_MARK_END = 7;
        //public const int THREAD = 8;
        
        struct Identifer
        {
            public int type;
            public string name;
            public bool isArgument;
            public bool isGlobal;
            //public void* data;
        };

        public IdentiferTable() 
        {
            //придумать что нить получше этогo
            m_reserved.Add("io");
            m_reserved.Add("select");
            m_reserved.Add("print");
            m_reserved.Add("type");            
        }

        List<Identifer> m_stack = new List<Identifer>();
        List<string> m_reserved = new List<string>();

        public void AddIdentiferList(string names, string values, bool isGlobal = true)
        {        
            string[] nameT = names.Split(' ');            
            //string[] valueT = values.Split(' ');
            foreach (string name in nameT)
            {
                if (name.Length > 0)
                {
                    AddIdentifer(name, isGlobal);
                }
            }
        }

        public void AddBlockStartMark()
        {
            Identifer id = new Identifer();
#if USING_DEBUG_NAMES
            id.name = "Начало блока";
#else
            id.name = "";
#endif
            id.isArgument = false;
            id.isGlobal = false;
            id.type = BLOCK_MARK_BEGIN;
            m_stack.Add(id);
        }

        public void AddBlockEndMark()
        {
            Identifer id = new Identifer();
#if USING_DEBUG_NAMES
            id.name = "Конец блока";
#else
            id.name = "";
#endif
            id.isArgument = false;
            id.isGlobal = false;
            id.type = BLOCK_MARK_END;
            m_stack.Add(id);
        }

        public void AddFunctionDefinition(string name, string args, bool isGlobal = true)
        {
            if (IsReservedByLanguage(name))
            {
                Console.WriteLine("Зарезервированный языком идентификатор: {0}", name);
                return;
            }
#if TRACE_ACTIONS
            if (args.Length > 0)
            {
                Console.WriteLine("Аргументы функции {0} : {1}", name, args);
            }
#endif
            Identifer i = new Identifer();
            i.name = name;
            i.type = FUNCTION;
            i.isArgument = false;
            i.isGlobal = isGlobal;
            m_stack.Add(i);
            
            string[] arguments= args.Split(' ');
            foreach (string a in arguments)
            {
                if (a.Length > 0)
                {
                    AddIdentifer(a, false, true);
                }
            }
        }

        public void AddIdentifer(string identifer, bool isGlobal = false, bool isArgument = false, int type = UNDEF)//, void * value)
        {
            //Зарезеривирован ли?
            if (IsReservedByLanguage(identifer))
            {
                Console.WriteLine("Зарезервированный языком идентификатор: {0}", identifer);
                return;
            }
            Identifer i = new Identifer();
            i.type = type;
            i.name = identifer;
            i.isGlobal = isGlobal;
            i.isArgument = isArgument;
            //i.data
            m_stack.Add(i);
        }

        public void ReturnFromFunction(string returnArg = "")
        {
            Identifer id = new Identifer();
#if USING_DEBUG_NAMES
            id.name = "Конец блока функции";
#else
            id.name = "";
#endif
            id.type = BLOCK_MARK_END;
            id.isArgument = false;
            id.isGlobal = false;
            m_stack.Add(id);
            /*if (returnArg.Equals(""))
            {
            }*/
           // RemoveLocals();
        }

        public void CheckIdentiferVisibility(string identifer)
        {
            int i = GetIdByName(identifer);
            if (i != -1)
            {
#if TRACE_ACTIONS
                Console.WriteLine("Идентификатор найден:{0}", identifer);
#endif
                return;
            }
            else
            {
                Console.WriteLine("Неизвестный идентификатор: {0}", identifer);
            }            
        }

        public void PrintTable()
        {
            foreach (Identifer i in m_stack)
            {
                if (i.type == FUNCTION)
                {
                    Console.WriteLine("Начало функции: {0}", i.name);
                }
                else
                    Console.WriteLine("{0}", i.name);
            }
        }
        
        private bool IsReservedByLanguage(string identifer)
        {
            foreach (string i in m_reserved)
            {
                if (i == identifer)
                {
                    return true;
                }
            }
            return false;
        }       
        
        private int GetIdByName(string identifer)
        {        
            int level = 0;
            bool currentFlag = true;
            //locals
            for (int i = m_stack.Count - 1; i > 0; i--)
            {
                Identifer id = m_stack[i];
                if (id.type == BLOCK_MARK_END)
                {
                    level++;
                }
                else if (id.type == BLOCK_MARK_BEGIN || id.type == FUNCTION)
                {
                    currentFlag = false;
                    level--;
                }
                if ((currentFlag && level == 0 ) || level < 0)
                {
                    if (id.name.Equals(identifer))
                    {
                        return i;
                    }
                }
            }
            //global
            Console.WriteLine("Поиск глобального идентификатора: {0}", identifer);
            for (int i = m_stack.Count - 1; i > 0; i--)
            {
                if (m_stack[i].name.Equals(identifer) && m_stack[i].isGlobal)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
