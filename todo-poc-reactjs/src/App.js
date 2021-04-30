import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Todos from "./components/todos";

import { initialState, todoReducer } from "./reducers/todo-reducer";
import { TodosProvider } from "./reducers/app-context";

const App = () => {
  const todoStoreConfig = {
    initialState,
    reducer: todoReducer,
  };
  return (
    <>
      <TodosProvider {...todoStoreConfig}>
        <Router>
        <Switch>
          <Route path="/">
            <Todos />
          </Route>
        </Switch>
      </Router>
      </TodosProvider>
    </>
  );
};

export default App;
