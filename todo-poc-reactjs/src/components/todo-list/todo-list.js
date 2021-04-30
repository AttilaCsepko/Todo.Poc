import TodoListItem from '../todo-list-item';
import { Card, Segment } from 'semantic-ui-react';

const TodoList = ({ todos, onDeleted, toggleDone, toggleImportant  }) => {
  
  return (
    <Segment>
      <Card.Group>
        {todos.map((item) => {
          const { id, ...itemProps } = item;
          return (
            <TodoListItem
              key={id}
              {...itemProps}
              onDeleted={() => onDeleted(id)}
              toggleImportant={() => toggleImportant(id)}
              toggleDone={() => toggleDone(id)}
            />
          );
        })}
      </Card.Group>
    </Segment>
  );
};

export default TodoList;