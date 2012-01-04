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

Var::~Var(void)
{
	if (m_type == UNDEF)
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
