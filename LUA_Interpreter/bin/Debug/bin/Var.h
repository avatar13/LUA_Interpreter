#pragma once
#include <string>
#include <sstream>

class Var
{
public:
	static const int UNDEF = 0;
	static const int DEF = 1;
	static const int INT = 2;
	static const int DOUBLE = 3;
	static const int STRING = 4;	
	
	Var(void);
	//Var(int type, void * value);
	// нужно для операций. Перегрузка конструктора копирования
	Var(int value);
	Var(double value);
	Var(char * str);
	Var(std::string const & str);

	Var operator +(Var & other);
	Var operator -(Var & other);
	Var operator *(Var & other);
	Var operator /(Var & other);
	Var operator ^(Var & other);
	Var operator %(Var & other);
	Var operator <(Var & other);
	Var operator >(Var & other);

	//конкатенация
	Var operator |(Var & other);

	Var operator ==(Var & other);
	//Var operator ~=(Var & other);
	Var operator >=(Var & other);
	Var operator <=(Var & other);

	void setDef();
	void setValue(int const & value);
	void setValue(double const & value);
	void setValue(const char * value);
	//void setValue(int const & value);

	~Var(void);
private:
	int getType();
	void Destroy();
	std::string toString();
	int toInt();
	double toNumber();

	
	int m_type;
	void* m_value;
};

