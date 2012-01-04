#include <stdio.h>

class ABS
{
	public : 
		ABS()
		{
			m_temp = "lola";
		}
		int getTemp()
		{
			return m_temp;
		}	
	private:
		string m_temp;
};

int main()
{
	ABS l;
	puts("HW");	
	puts(l.getTemp());
	return 0;
}