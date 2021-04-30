import { useEffect, useState } from 'react';

const useGetAllTodosApi = (refresh) => {
  const [todos, setTodos] = useState([]);
  const [isLoaded, setIsLoaded] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {

    fetch(`http://localhost:7075/api/todo`)
      .then((res) => res.json())
      .then(
        (response) => {
          setTodos(response);
          setIsLoaded(true);
        },
        (error) => {
          setError(error);
          setIsLoaded(true);
        }
      );
  }, [refresh]);


  
  return {
    todos,
    isLoaded,
    error,
  };
};

export default useGetAllTodosApi;