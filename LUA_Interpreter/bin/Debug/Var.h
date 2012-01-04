#pragma once
#include <string>
#include <sstream>

class Var
{
public:
	static const int UNDEF = 0;
	static const int INT = 1;
	static const int DOUBLE = 2;
	static const int STRING = 3;
	
	Var(void);
	//Var(int type, void * value);	
	Var(int value);
	Var(double value);
	Var(char * str);
	Var operator +(Var & other);
	Var operator -(Var & other);
	Var operator *(Var & other);
	Var operator /(Var & other);
	~Var(void);
private:
	std::string toString();
	double toNumber();

	
	int m_type;
	void* m_value;
};

