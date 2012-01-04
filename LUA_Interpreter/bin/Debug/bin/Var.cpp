#include "StdAfx.h"
#include "Var.h"

using namespace std;
Var::Var(void)
{
	m_type = UNDEF;
}

Var::Var(int value)
{
	m_type = INT;
	m_value = new int(value);
}

Var::Var(char * str)
{
	m_type = STRING;
	m_value = new string(str);
}

Var::Var(double value)
{
	m_type = DOUBLE;
	m_value = new double(value);
}

Var::Var(std::string const & str)
{
	m_type = STRING;
	m_value = new string(str);
}
/*
Var::Var(int type, void * value)
{
	m_type = type;
	m_value = value;
}
*/

string Var::toString()
{
	if (m_type == STRING)
	{
		string * s = (string *)m_value;
		return *s;
	}
	else
	{	
		double * d = (double *)m_value;
		ostringstream sstream;
		if (!(sstream << (*d)))
		{
			throw new std::bad_cast("Can`t convert to string");
		}
		return string(sstream.str());
	}
}

double Var::toNumber()
{
	if (m_type == DOUBLE)
	{
		double * d = (double *)m_value;
		return *d;
	}
	else if (m_type == STRING)
	{
		string * s = (string *)m_value;
		double num = atof(s->c_str());
		return num;
	}
}

int Var::toInt()
{
	if (m_type == INT)
	{
		return *(int *)m_value;
	}
	else
	{
	}
}

Var Var::operator +(Var & other)
{
	return toNumber() + other.toNumber();
}

Var Var::operator -(Var & other)
{
	return toNumber() - other.toNumber();
}

Var Var::operator /(Var & other)
{
	return toNumber() / other.toNumber();
}

Var Var::operator *(Var & other)
{
	return toNumber() * other.toNumber();
}

Var Var::operator ^(Var & other)
{
	return pow(toNumber(), other.toNumber());	
}

Var Var::operator %(Var & other)
{
	return toInt() % other.toInt();
}

Var Var::operator >(Var & other)
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

Var Var::operator <(Var & other)
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

Var Var::operator ==(Var & other)
{
	if (getType() == STRING && other.getType() == STRING)
	{
		if (toString().compare(other.toString()) == 0)
		{
			return toString();
		}
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

Var Var::operator >=(Var & other)
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

Var Var::operator <=(Var & other)
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

Var Var::operator |(Var & other)
{
	return string(toString() + other.toString());
}

void Var::setValue(int const & value)
{
	if (m_value != NULL)
	{
		Destroy();
	}	
	m_value = new int(value);
	m_type = INT;	
}

void Var::setValue(double const & value)
{
	if (m_value != NULL)
	{
		Destroy();
	}	
	m_value = new double(value);
	m_type = DOUBLE;	
}

void Var::setValue(const char * value)
{
	if (m_value != NULL)
	{
		Destroy();
	}	
	m_value = new string(value);
	m_type = STRING;	
}

void Var::setDef()
{
	m_type = DEF;
}

int Var::getType()
{
	return m_type;
}

void Var::Destroy()
{
	if (m_type == UNDEF || m_type == DEF)
	{
		return;
	}
	else if (m_type == INT)
	{
		delete (int *)m_value;
	}	
	else if (m_type == STRING)
	{
		delete (string *)m_value;
	}
	else if (m_type == DOUBLE)
	{
		delete (double *)m_value;
	}
}

Var::~Var(void)
{
	Destroy();
}
