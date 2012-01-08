void write(Var & v)
{
	v.printValue();
}

void writeln(Var & v)
{
	v.printValue();
	printf("\n");
}

void write(char * str)
{
	printf("%s", str);
}

void writeln(char * str)
{
	printf("%s\n", str);
}

void write(int const & i)
{
	printf("%d", i);
}

void writeln(int const & i)
{
	printf("%d\n", i);
}

void write(double const & d)
{
	printf("%f", d);
}

void writeln(double const & d)
{
	printf("%f\n", d);
}