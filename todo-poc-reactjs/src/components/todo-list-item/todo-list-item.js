import { Component } from 'react';
import { Button, Card } from 'semantic-ui-react'
class TodoListItem extends Component {

  render() {
    const {
      taskDescription,
      isCompleted,
      onDeleted,
      toggleImportant,
      toggleDone } = this.props;

    return (
      <Card>
        <Card.Content>
          <Card.Header>Todo Item</Card.Header>
          <Card.Description>{taskDescription}</Card.Description>
        </Card.Content>
        <Card.Content extra>
          <div className="ui three buttons">
            <Button basic color="blue" onClick={toggleDone}>
            <i className= {isCompleted? "fa fa-check" : "fa fa-spinner"} />
             
            </Button>
            <Button basic color="green" onClick={toggleImportant}>
              <i className="fa fa-exclamation" />
            </Button>
            <Button basic color="red" onClick={onDeleted}>
              <i className="fa fa-trash-o" />
            </Button>
          </div>
        </Card.Content>
      </Card>
    );
  };
}
export default TodoListItem;