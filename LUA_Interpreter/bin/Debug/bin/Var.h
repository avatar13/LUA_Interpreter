#include <stdlib.h>
#include <string.h>

class Var
{
public:
	static const int UNDEF = 0;
	static const int DEF = 1;
	static const int INT = 2;
	static const int DOUBLE = 3;
	static const int STRING = 4;	
	static const int TABLE = 5;
	
	inline Var(void)
	{		
		m_type = UNDEF;		
	}

	//Var(int type, void * value);
	// нужно для операций. Перегрузка конструктора копирования
	inline Var(int value)
	{
		m_type = INT;
		m_value = new int(value);
	}

	inline Var(double value)
	{
		m_type = DOUBLE;
		m_value = new double(value);
	}

	inline Var(const char * str)
	{
		m_type = STRING;
		//m_value = new char(str);
	}
	//Var(std::string const & str);

	Var operator +(Var & other)
	{
		return toNumber() + other.toNumber();
	}

	Var operator -(Var & other)
	{
		return toNumber() - other.toNumber();
	}

	Var operator *(Var & other)
	{
		return toNumber() * other.toNumber();
	}

	Var operator /(Var & other)
	{
		/*
		if (other.toNumber() == 0)
		{
			//throw new 			
		}*/
		return toNumber() / other.toNumber();
	}

	Var operator ^(Var & other)
	{
		//return pow(toNumber(), other.toNumber());
		//return Var("123");
		//return "123";
		return Var("123");
	}

	Var operator %(Var & other)
	{
		if (other.toNumber() == 0)
		{
			return "";
		}
		return toInt() % other.toInt();
	}

	Var operator <(Var & other)
	{
		if (getType() == DOUBLE && other.getType() == DOUBLE)	
		{
			if (toNumber() < other.toNumber())
			{
				return toNumber();
			}
			return Var();
		}
		else if(getType() == STRING && other.getType() == STRING)
		{
			/*if (string.compare()
			{
			}
			return Var();*/
		}
		else if (getType() == INT < other.getType() == INT)
		{
			if (toInt() < other.toInt())
			{
				return toInt();
			}
			return Var();
		}
	}

	Var operator >(Var & other)
	{
		if (getType() == INT && other.getType() == INT)
		{
			if (toInt() > other.toInt())
			{
				return toInt();
			}
			return Var();
		}
		else if(getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() > other.toNumber())
			{
				return toNumber();
			}
			return Var();
		}
		else if(getType() == STRING && other.getType() == STRING)
		{

		}
		else
		{
		}
	}

	//конкатенация
	Var operator |(Var & other)
	{
		//return string(toString() + other.toString());
		return "";
	}

	//работа с метатаблицами	
	Var & operator [](int const & index);
	Var & operator [](Var const & index);

	Var operator ==(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			/*if (toString().compare(other.toString()) == 0)
			{
				return toString();
			}*/
			return Var();
		}
		else if (getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() == other.toNumber())
			{
				return toNumber();
			}
			return Var();
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toInt() == other.toInt())
			{
				return toInt();
			}
			return Var();
		}
	}

	//Var operator ~=(Var & other);
	Var operator >=(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			/*if(toString().co)
			{
			}
			return Var();*/
		}
		else if (getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() >= other.toNumber())
			{
				return toNumber();
			}
			return Var();
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toNumber() >= other.toNumber())
			{
				return toInt();
			}
			return Var();
		}
		else
		{
		}
	}

	Var operator <=(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			/*if ()
			{
			}
			return Var();*/
		}
		else if (getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() <= other.toNumber())
			{
				return toNumber();
			}
			return Var();
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toInt() <= other.toInt())
			{
				return toInt();
			}
			return Var();
		}
		else
		{
		}
	}

	void setDef()
	{
		m_type = DEF;
	}

	void setValue(Var & value)
	{
		Destroy();
		if (value.getType() == STRING)
		{		
			m_type = STRING;
			//m_value = new string(*(string *)value.m_value);
		}
		else if (value.getType() == INT)
		{
			m_type = INT;
			m_value = new int(*(int *)value.m_value);		
		}
		else if (value.getType() == DOUBLE)
		{
			m_type = DOUBLE;
			m_value = new double(*(double *)value.m_value);
		}
		else if (value.getType() == TABLE)
		{
			m_type = TABLE;
			//m_value = 		
		}
	}

	void setValue(int const & value)
	{		
		Destroy();	
		m_value = malloc(sizeof(int));
		int * pI = (int *)m_value;
		pI[0] = value;		
		m_type = INT;
	}

	void setValue(double const & value)
	{
		Destroy();
		m_value = malloc(sizeof(double));
		double * temp = (double *)m_value;
		temp[0] = value;
		m_type = DOUBLE;
	}

	void setValue(const char * value)
	{
		Destroy();
		m_value = malloc(sizeof(char) * strlen(value));
		char * str = (char *)m_value;		
		memcpy(str, value, strlen(value));	
		m_type = STRING;
	}

	void printValue()
	{
		if (m_type == INT)
		{
			int * val = (int *)m_value;
			printf("Value: %d\n", *val);
		}
		else if (m_type == DOUBLE)
		{
			double * val = (double *)m_value;
			printf("Value: %f\n", *val);
		}
		else if (m_type == STRING)
		{
			puts((char *)m_value);
		}
	}

	~Var(void)
	{		
		Destroy();		
	}

private:
	int getType()
	{
		return m_type;
	}

	void Destroy()
	{
		printf("%d\n", m_type);
		if (m_type == UNDEF || m_type == DEF)
		{
			printf("NOTHING TO DELETE\n");
			return;
		}
		else if (m_type == INT)
		{			
			free((int *)m_value);
		}	
		else if (m_type == STRING)
		{
			//free(()m_value);
		}
		else if (m_type == DOUBLE)
		{			
			free((double *)m_value);
		}
		else if (m_type == TABLE)
		{
			//delete ( *)m_value;
		}
	}

	//std::string toString();
	int toInt()
	{
		return 1;
	}

	double toNumber()
	{
		return 2;
	}

	
	int m_type;
	void* m_value;
};

