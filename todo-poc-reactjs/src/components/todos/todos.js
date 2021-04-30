import { useEffect } from "react";
import TodoList from "../todo-list";
import { Button, Segment } from "semantic-ui-react";
import { useTodos } from "../../reducers/app-context";
import TodoAddForm from "../todo-add-form";
import {ACTION_TYPES} from "../../actions/todo-actions"
const Todos = () => {
  const axios = require("axios");
  const [todos, dispatchTodos] = useTodos();

  useEffect(() => {
    handleRefresh();
  }, []);

  const handleRefresh = () => {
    const fetchUrl = process.env.REACT_APP_TODO_BACKEND_URL + "todo/";

    axios.get(fetchUrl).then((response) => {
      dispatchTodos({
        type: ACTION_TYPES.FETCH_ALL,
        payload: response.data,
      });
    });
  };

  const handleAdd = (taskDescription, importance) => {
    const createUrl = process.env.REACT_APP_TODO_BACKEND_URL + `todo/`;

    const newTodoItem = { taskDescription, importance };

    axios.post(createUrl, JSON.stringify(newTodoItem)).then((response) => {
      console.log(response.data);
      dispatchTodos({
        type: ACTION_TYPES.CREATE,
        payload: response.data,
      });
    });
  };

  const handleDelete = (id) => {
    const deleteUrl = process.env.REACT_APP_TODO_BACKEND_URL + `todo/${id}`;

    axios.delete(deleteUrl).then((response) => {
      console.log(response.data);
    });

    dispatchTodos({
      type: ACTION_TYPES.DELETE,
      payload: id,
    });
  };
  const handleToggleDone = (id) => {
    const updateUrl = process.env.REACT_APP_TODO_BACKEND_URL + `todo/${id}`;

    const todoItem = todos.list.filter((el) => el.id === id)[0];
    todoItem.isCompleted = !todoItem.isCompleted;

    axios.put(updateUrl, JSON.stringify(todoItem)).then((response) => {
      console.log(response.data);
    });
    dispatchTodos({
      type: ACTION_TYPES.UPDATE,
      payload: todoItem,
    });
  };

  const handleToggleImportant = (id) => {
    console.log(`NOT IMPLEMENTED ON SERVER-SIDE: toggle important ${id}`);
  };

  return (
    <>
      <Segment>
        <Button color="green" onClick={handleRefresh}>
          <i className="fa fa-refresh" />
        </Button>
      </Segment>

      <Segment>
        <TodoAddForm onAdded={handleAdd} />
      </Segment>
      <TodoList
        todos={todos.list}
        onDeleted={handleDelete}
        toggleImportant={handleToggleImportant}
        toggleDone={handleToggleDone}
      />
    </>
  );
};
export default Todos;
