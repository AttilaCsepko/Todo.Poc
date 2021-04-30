import { useState } from 'react';
import TodoList from '../todo-list';
import useGetAllTodosApi from '../../hooks/use-get-all-todos-api'
import { Button } from 'semantic-ui-react'

const Todos = () => {
  const [refresh, setRefresh] = useState(true);
  
  const { todos, isLoaded, error } = useGetAllTodosApi(refresh);

  const handleRefresh = () => {
    setRefresh(!refresh);
    console.log("refresh")
  }

   const handleDelete = (id) => {
     console.log(`delete ${id}`);
   };
   const handleToggleImportant = (id) => {
     console.log(`toggle important ${id}`);
   };
   const handleToggleDone = (id) => { console.log(`toggle done ${id}`);}

  return (
    <>
      <Button color="green" onClick={handleRefresh}>
        <i className="fa fa-refresh" />
      </Button>
      <TodoList
        todos={error? [] : todos}
        isLoaded={isLoaded}
        onDeleted={handleDelete}
        toggleImportant={handleToggleImportant}
        toggleDone={handleToggleDone}
      />
    </>
  );
}
export default Todos;