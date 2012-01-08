class Array
{
public:

	struct Node
	{
		Var key, value;
	};

	static const int INDEXED = 0;
	static const int ASSOC = 1;
	Array(void)
	{
		
	}

	void CreateIndexedArray(Var const & arr[])
	{
		int size = sizeof(arr) / sizeof(arr[0]);
		m_arrayData = malloc(sizeof(Node) * size);		
		memcpy(m_arrayData, &arr, sizeof(Node) * size);
		m_type = INDEXED;
	}

	void CreateAssocArray(Var const & keys[], Var const & values[])
	{
		int sizeK = sizeof(keys) / sizeof(keys[0]);
		int sizeV = sizeof(values) / sizeof(values[0]);
	}

	void Dispose()
	{
		if (m_type == IDEXED)
		{
			free((Var *)m_arrayData);
		}
	}

	~Array()
	{
		Dispose();
	}
private:
	int m_type;
	void * m_arrayData;
};