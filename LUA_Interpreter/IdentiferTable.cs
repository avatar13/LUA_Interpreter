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
        //public const int 
        //public const int THREAD = 8;        

        struct Identifer
        {
            public int type;
            public string name;
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

        public void AddIdentiferList(string names, string values)
        {        
            string[] nameT = names.Split(' ');            
            //string[] valueT = values.Split(' ');
            foreach (string name in nameT)
            {
                if (name.Length > 0)
                {
                    AddIdentifer(name);
                }
            }
        }

        public void AddFunctionDefinition(string name, string args)
        {
            if (IsReservedByLanguage(name))
            {
                Console.WriteLine("Зарезервированный языком идентификатор: {0}", name);
                return;
            }

            Console.WriteLine("@@@@: {0}", args);

            Identifer i = new Identifer();
            i.name = name;
            i.type = FUNCTION;
            m_stack.Add(i);
            
            string[] arguments= args.Split(' ');
            foreach (string a in arguments)
            {
                if (a.Length > 0)
                {
                    AddIdentifer(a);
                }
            }
        }

        public void AddIdentifer(string identifer, int type = UNDEF)//, void * value)
        {
            Console.WriteLine(identifer);
            if (IsReservedByLanguage(identifer))
            {
                Console.WriteLine("Зарезервированный языком идентификатор: {0}", identifer);
                return;
            }

            int t = GetIdByName(identifer);
            if (t != -1)
            {
                Console.WriteLine("Уже присутствует идентификатор: {0}", identifer);
                return;
            }
            else
            {
                Identifer i = new Identifer();
                i.type = type;
                i.name = identifer;
                //i.data
                m_stack.Add(i);
            }
        }

        public void ReturnFromFunction(string returnArg = "")
        {
            /*if (returnArg.Equals(""))
            {
            }*/
           // RemoveLocals();
        }

        public void CheckIdentiferVisibility(string identifer)
        {
            if (IsIdentiferValid(identifer))
            {
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
                Console.WriteLine("{0}", i.name);
            }
        }

        //проверка идентификатора на объявление
        private bool IsIdentiferValid(string identifer)
        {
            //проверяем глобальный уровень
            for (int i = 0; i < m_stack.Count; i++)
            {
                if (m_stack[i].name.Equals(identifer))
                {
                    return true;
                }
                else if (m_stack[i].type == FUNCTION)
                {
                    break;
                }
            }
            //проверяем локальный уровень
            for (int i = GetLastMark(); i < m_stack.Count; i++)
            {
                if (m_stack[i].name.Equals(identifer))
                {
                    return true;
                }
            }
            return false;
        }

        private void RemoveLocals()
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!RemoveLocals");
            int m = GetLastMark();
            m_stack.RemoveRange(m, m_stack.Count - m);
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

        //получаем последнюю функцию
        private int GetLastMark()
        {
            for (int i = m_stack.Count - 1; i > 0; i--)
            {
                if (m_stack[i].type == FUNCTION)
                {
                    return i;
                }
            }
            return 0;
        }

        private int GetIdByName(string identifer)
        {           
            for (int i = 0; i < m_stack.Count; i++)
            {
                if (m_stack[i].name.Equals(identifer))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
