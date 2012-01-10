#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
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
	
	inline Var(void):m_isCopy(false), m_isDestroy(true)
	{		
		m_type = UNDEF;		
	}
		
	// нужно для операций. Перегрузка конструктора копирования
	Var(int value):m_isCopy(false), m_isDestroy(true)
	{
		m_type = INT;
		m_value = malloc(sizeof(int));
		int * pI = (int *)m_value;
		pI[0] = value;
	}

	Var(double value):m_isCopy(false), m_isDestroy(true)
	{
		m_type = DOUBLE;
		m_value = malloc(sizeof(double));
		double * pD = (double *)m_value;
		pD[0] = value;
	}

	Var(const char * str):m_isCopy(false), m_isDestroy(true)
	{
		m_type = STRING;		
		m_value = malloc(sizeof(char) * strlen(str));		
		strcpy((char *)m_value, str);		
	}
	
	Var(Var const & other):m_value(other.m_value), m_type(other.m_type), m_size(other.m_size), m_isDestroy(false)
	{		
		m_isCopy = true;		
	}
	
	/*
	Var operator =(Var other)
	{
		Destroy();
		m_type = other.getType();
		if (m_type == INT)
		{
			m_value = malloc(sizeof(int));
			int * p = (int *)m_value;
			p[0] = other.toInt();
		}
		else if (m_type == STRING)
		{
			m_value = malloc(sizeof(other.toString()));
			memcpy(m_value, other.toString(), sizeof(other.toString()));			
		}
		else if (m_type == DOUBLE)
		{
			m_value = malloc(sizeof(double));
			double * p = (double *)m_value;
			p[0] = other.toNumber();
		}
		else if (m_type == TABLE)
		{
			m_value = malloc(other.getSize() * sizeof(Var));
			memcpy(m_value, other.getRawData(), other.getSize() * sizeof(Var));			
			m_size = other.getSize();
		}
	}
	*/
	Var operator +(Var other)
	{
		if (m_type == INT && other.getType() == INT)
		{
			return toInt() * other.toInt();
		}
		return toNumber() + other.toNumber();
	}

	Var operator -(Var & other)
	{
		if (m_type == INT && other.getType() == INT)
		{
			return toInt() * other.toInt();
		}
		return toNumber() - other.toNumber();
	}

	Var operator *(Var & other)
	{
		return toNumber() * other.toNumber();
	}

	Var operator /(Var & other)
	{		
		if (other.toNumber() == 0)
		{
			return Var();
		}
		return toNumber() / other.toNumber();
	}

	Var operator ^(Var other)
	{
		return pow(toNumber(), other.toNumber());		
	}
	
	Var operator %(Var & other)
	{
		if (other.toNumber() == 0)
		{
			return Var();
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
		else if (getType() == INT && other.getType() == INT)
		{
			if (toInt() < other.toInt())
			{
				return toInt();
			}			
		}
		else if ((m_type == INT && other.getType() == DOUBLE) || 
				(m_type == DOUBLE && other.getType() == INT))
		{
			if(toNumber() < other.toNumber())
			{
				return toNumber();
			}
		}
		if ((m_type == INT || m_type == DOUBLE) && other.getType() == STRING)
		{
			if (toNumber() < other.toNumber())
			{
				return toNumber();
			}
		}
		if (m_type == STRING && (other.getType() == DOUBLE || other.getType() == INT))
		{
			if (strlen(toString()) < strlen(other.toString()))
			{
				return toString();
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
		else if ((m_type == INT && other.getType() == DOUBLE) || 
				(m_type == DOUBLE && other.getType() == INT))
		{
			if(toNumber() > other.toNumber())
			{
				return toNumber();
			}
		}
		if ((m_type == INT || m_type == DOUBLE) && other.getType() == STRING)
		{
			if (toNumber() > other.toNumber())
			{
				return toNumber();
			}
		}
		if (m_type == STRING && (other.getType() == DOUBLE || other.getType() == INT))
		{
			if (strlen(toString()) > strlen(other.toString()))
			{
				return toString();
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
	Var & operator [](int const & index)
	{
		if (m_type == TABLE)
		{
			Var * pI = (Var *)m_value;			
			if (0 <= index && index < m_size)
			{
				return pI[index];
			}
		}
	}
	
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
		}
		else if (getType() == INT && other.getType() == INT)
		{
			if (toInt() == other.toInt())
			{
				return toInt();
			}			
		}
		else if ((m_type == INT && other.getType() == DOUBLE) || 
				(m_type == DOUBLE && other.getType() == INT))
		{
			if(toNumber() == other.toNumber())
			{
				return toNumber();
			}
		}
		return Var();
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
		else if ((m_type == INT && other.getType() == DOUBLE) || 
				(m_type == DOUBLE && other.getType() == INT))
		{
			if(toNumber() >= other.toNumber())
			{
				return toNumber();
			}
		}
		if ((m_type == INT || m_type == DOUBLE) && other.getType() == STRING)
		{
			if (toNumber() >= other.toNumber())
			{
				return toNumber();
			}
		}
		if (m_type == STRING && (other.getType() == DOUBLE || other.getType() == INT))
		{
			if (strlen(toString()) >= strlen(other.toString()))
			{
				return toString();
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
		else if ((m_type == INT && other.getType() == DOUBLE) || 
				(m_type == DOUBLE && other.getType() == INT))
		{
			if(toNumber() <= other.toNumber())
			{
				return toNumber();
			}
		}
		if ((m_type == INT || m_type == DOUBLE) && other.getType() == STRING)
		{
			if (toNumber() <= other.toNumber())
			{
				return toNumber();
			}
		}
		if (m_type == STRING && (other.getType() == DOUBLE || other.getType() == INT))
		{
			if (strlen(toString()) <= strlen(other.toString()))
			{
				return toString();
			}
		}
		return Var();
	}

	void setDef()
	{
		m_type = DEF;
	}

	void * getRawData()
	{
		return m_value;
	}

	int getSize()
	{
		return m_size;
	}
	
	void setValue(Var value)
	{		
		Destroy();
		if (value.getType() == STRING)
		{		
			m_type = STRING;			
			m_value = malloc(sizeof(char) * strlen(value.toString()));			
			strcpy((char *)m_value, value.toString());
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
			m_value = malloc(value.getSize() * sizeof(Var));
			Var * p = (Var *)m_value;
			for (int i = 0; i < value.getSize(); i++)
			{
				Var v;
				v.disableAutoDestroy();				
				if (value[i].getType() == INT)
				{					
					int * pI = (int *)value[i].getRawData();
					v.setValue(pI[0]);					
				}
				else if (value[i].getType() == DOUBLE)
				{
					double * pD = (double *)value[i].getRawData();
					v.setValue(pD[0]);
				}
				else if (value[i].getType() == STRING)
				{
					char * str = (char *)value[i].getRawData();
					v.setValue(str);
				}
				else if (value[i].getType() == TABLE)
				{
					int size = value[i].getSize();
					v.setValue(value[i]);
				}
				p[i] = v;
			}			
			m_size = value.getSize();
		}
	}
	/*
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
			m_value = malloc(value.getSize() * sizeof(Var));
			memcpy(m_value, value.getRawData(), value.getSize() * sizeof(Var));
			m_size = value.getSize();
		}
	}
	*/
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

	void setValue(Var arr[], int size)
	{
		Destroy();
		int cur_size = 0;
		m_value = malloc(sizeof(Var) * size);
		Var * p = (Var *)m_value;		
		m_type = TABLE;
		m_size = size;
		for (int i = 0; i < size; i++)
		{			
			Var v;
			v.disableAutoDestroy();
			if (arr[i].getType() == INT)
			{
				int * pI = (int *)arr[i].getRawData();
				v.setValue(pI[0]);			
			}
			else if (arr[i].getType() == DOUBLE)
			{
				double * pD = (double *)arr[i].getRawData();
				v.setValue(pD[0]);
				p[i] = v;	
			}
			else if (arr[i].getType() == STRING)
			{
				char * str = (char *)arr[i].getRawData();
				v.setValue(str);
				p[i] = v;			
			}
			p[i] = v;
		}		
	}
	
	void printValue()
	{
		if (m_type == INT)
		{
			int * val = (int *)m_value;
			printf("%d", *val);
		}
		else if (m_type == DOUBLE)
		{
			double * val = (double *)m_value;
			printf("%f", *val);
		}
		else if (m_type == STRING)
		{
			printf("%s", (char *)m_value);			
		}
		else if (m_type == TABLE)
		{
			Var * arr = (Var *)m_value;	
			printf("{");
			for (int i = 0; i < m_size; i++)
			{
				arr[i].printValue();
				printf(" ");
			}
			printf("}");
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
			double * pD = (double *)m_value;
			return pD[0];
		}
		else if (m_type == INT)
		{
			int * pI = (int *)m_value;
			return pI[0];
		}
		return -1;
	}

	double toNumber()
	{
		if (m_type == STRING)
		{
			return atof((char *) m_value);
		}
		else if(m_type == INT)
		{			
			int * pI = (int *)m_value;
			return pI[0];
		}
		else if (m_type == DOUBLE)
		{
			double * pD = (double *)m_value;
			return pD[0];
		}
		return 0;
	}

	void disableAutoDestroy()
	{
		m_isDestroy = false;
	}

	void Destroy()
	{
		if (m_type == UNDEF || m_type == DEF)
		{			
			return;
		}
		else if (m_type == INT)
		{
			int * p = (int *)m_value;
			free((int *)m_value);
		}	
		else if (m_type == STRING)
		{
			free((char *)m_value);
		}
		else if (m_type == DOUBLE)
		{
			double * p = (double *)m_value;
			free((double *)m_value);
		}
		else if (m_type == TABLE)
		{
			Var * p = (Var *)m_value;
			for (int i = 0; i < m_size; i++)
			{
				p[i].Destroy();
			}
			free(p);
		}
		m_value = NULL;
	}

	~Var(void)
	{
		if (!m_isCopy && m_isDestroy)
		{			
			Destroy();		
		}
	}

private:
	int getType()
	{
		return m_type;
	}

	bool m_isDestroy;
	bool m_isCopy;
	int m_type;
	int m_size;
	void* m_value;
};

