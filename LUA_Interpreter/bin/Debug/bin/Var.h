#include <stdlib.h>
#include <stdio.h>
#include <string.h>
//#include "array.h"

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
		
	// нужно для операций. Перегрузка конструктора копирования
	Var(int value)
	{
		m_type = INT;
		m_value = malloc(sizeof(int));
		int * pI = (int *)m_value;
		pI[0] = value;
	}

	Var(double value)
	{
		m_type = DOUBLE;
		m_value = malloc(sizeof(double));
		double * pD = (double *)m_value;
		pD[0] = value;
	}

	Var(const char * str)
	{
		m_type = STRING;		
		m_value = malloc(sizeof(char) * strlen(str));		
		strcpy((char *)m_value, str);		
	}

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
		}
		else if(getType() == STRING && other.getType() == STRING)
		{
			if (toString() < other.toString())
			{
				return Var(1);
			}
		}
		else if (getType() == INT < other.getType() == INT)
		{
			if (toInt() < other.toInt())
			{
				return toInt();
			}			
		}
		return Var();
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
			if (strlen(toString()) > strlen(other.toString()))
			{
				return Var(1);
			}
		}
		return Var();
	}

	//конкатенация
	Var operator |(Var & other)
	{
		if (m_type == STRING && other.getType() == STRING)
		{
			char * temp = (char *)malloc(sizeof(char) * (strlen(toString()) + strlen(other.toString())));			
			temp = strcpy(temp, toString());			
			temp = strcat(temp, other.toString());			
			temp[strlen(toString()) + strlen(other.toString())] = '\0';
			Var v;
			v.setValue(temp);			
			free(temp);			
			return v.toString();
		}
		else
		{
			return Var(0);
		}
	}

	//работа с метатаблицами	
	Var & operator [](int const & index);
	Var & operator [](const char * index);
	Var & operator [](Var const & index);

	Var operator ==(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			if (strcmp(toString(), other.toString()) == 0)
			{
				return Var(1);
			}
			Var p;
			p.setDef();
			return p;
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
		
	Var operator >=(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			if(toString() >= other.toString())
			{
				return Var(1);
			}			
		}
		else if (getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() >= other.toNumber())
			{
				return toNumber();
			}			
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toNumber() >= other.toNumber())
			{
				return toInt();
			}			
		}
		return Var();
	}

	Var operator <=(Var & other)
	{
		if (getType() == STRING && other.getType() == STRING)
		{
			if(strlen(toString()) <= strlen(other.toString()))
			{
				return Var(1);
			}
		}
		else if (getType() == DOUBLE && other.getType() == DOUBLE)
		{
			if (toNumber() <= other.toNumber())
			{
				return toNumber();
			}			
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toInt() <= other.toInt())
			{
				return toInt();
			}			
		}
		return Var();
	}

	void setDef()
	{
		m_type = DEF;
	}

	void setValue(Var value)
	{
		Destroy();
		if (value.getType() == STRING)
		{		
			m_type = STRING;			
			m_value = malloc(sizeof(char) * strlen(value.toString()));
			memcpy(m_value, value.toString(), strlen(value.toString()));
		}
		else if (value.getType() == INT)
		{
			m_type = INT;
			m_value = malloc(sizeof(int));
			int * pI = (int*)m_value;
			pI[0] = value.toInt();
		}
		else if (value.getType() == DOUBLE)
		{
			m_type = DOUBLE;
			m_value = malloc(sizeof(double));
			double * pD = (double*)m_value;
			pD[0] = value.toNumber();
		}
		else if (value.getType() == TABLE)
		{
			m_type = TABLE;
			//m_value =	
		}
	}

	void setValue(Var & value)
	{
		Destroy();
		if (value.getType() == STRING)
		{		
			m_type = STRING;			
			m_value = malloc(sizeof(char) * strlen(value.toString()));
			memcpy(m_value, value.toString(), strlen(value.toString()));
		}
		else if (value.getType() == INT)
		{
			m_type = INT;
			m_value = malloc(sizeof(int));
			int * pI = (int*)m_value;
			pI[0] = value.toInt();
		}
		else if (value.getType() == DOUBLE)
		{
			m_type = DOUBLE;
			m_value = malloc(sizeof(double));
			double * pD = (double*)m_value;
			pD[0] = value.toNumber();
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
		strcpy(str, value);
		m_type = STRING;
	}

	void setValue(int const arr[])
	{

	}
	
	void printValue()
	{
		if (m_type == INT)
		{
			int * val = (int *)m_value;
			printf("Value: %d", *val);
		}
		else if (m_type == DOUBLE)
		{
			double * val = (double *)m_value;
			printf("Value: %f", *val);
		}
		else if (m_type == STRING)
		{
			puts((char *)m_value);
		}
	}
	
	char * toString()
	{
		if (m_type == INT)
		{			
			char * str = (char *)malloc(sizeof(char) * 20);
			int *pI = (int *)m_value;
			sprintf(str, "%d", pI[0]);
			Destroy();
			m_value = (void *)str;			
			m_type = STRING;
			return (char *)m_value;
		}
		else if (m_type == DOUBLE)
		{
			char * str = (char *)malloc(sizeof(char) * 20);
			double * pD = (double *)m_value;			
			sprintf(str, "%f", pD[0]);
			Destroy();
			m_value = (void *)str;
			m_type = STRING;
			return (char *)m_value;
		}
		else if (m_type == STRING)
		{
			return (char *)m_value;
		}
		return Var("").toString();		
	}

	int toInt()
	{
		if (m_type == STRING)
		{
			return atoi((char *)m_value);
		}
		else if (m_type == DOUBLE)
		{
			return int(*(double *)m_value);
		}
		else if (m_type == INT)
		{
			int * pI = (int *)m_value;
			return pI[0];
		}
		return 0;
	}

	double toNumber()
	{
		if (m_type == STRING)
		{
			return atof((char *) m_value);
		}
		else if(m_type == INT)
		{
			return double(*(int *)m_value);
		}
		else if (m_type == DOUBLE)
		{
			double * pD = (double *)m_value;
			return pD[0];
		}
		return 0;
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

	void ConvertToString(int i)
	{
		
	}

	void Destroy()
	{		
		if (m_type == UNDEF || m_type == DEF)
		{			
			return;
		}
		else if (m_type == INT)
		{			
			free((int *)m_value);
		}	
		else if (m_type == STRING)
		{
			free((char *)m_value);
		}
		else if (m_type == DOUBLE)
		{			
			free((double *)m_value);
		}
		else if (m_type == TABLE)
		{
			//free(()m_value);
		}
	}

	int m_type;
	void* m_value;
};

