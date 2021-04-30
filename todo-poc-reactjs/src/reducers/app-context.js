import React from "react";

const TodosStore = React.createContext();
TodosStore.displayName = "Todo items store";
export const useTodos = () => React.useContext(TodosStore);

export const TodosProvider = ({ children, initialState, reducer }) => {
  const [todos, dispatchTodos] = React.useReducer(reducer, initialState);
  return (
    <TodosStore.Provider value={[todos, dispatchTodos]}>
      {children}
    </TodosStore.Provider>
  );
};
