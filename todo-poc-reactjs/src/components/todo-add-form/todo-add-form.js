import { Component } from "react";
import { Button, Card, Input } from "semantic-ui-react";

class TodoAddForm extends Component {
  state = {
    newTaskDescription: "",
    important: false,
  };

  onInputTextChange = (e) => {
    this.setState({
      newTaskDescription: e.target.value,
    });
  };

  onSubmit = (e) => {
    e.preventDefault();

    const { newTaskDescription } = this.state;
    if (newTaskDescription.length === 0) {
      return;
    }

    const { onAdded } = this.props;
    onAdded(this.state.newTaskDescription, this.state.important);

    this.setState({
      newTaskDescription: "",
      important: false,
    });
  };

  toggleImportant = (e) => {
    e.preventDefault();
    this.setState((state) => {
      return {
        important: !state.important,
      };
    });
  };

  render() {
    return (
      <form className="item-add-form d-flex" onSubmit={this.onSubmit}>
        <Card color='blue'>
          <Card.Content>
            <Card.Header>Create New Todo Item</Card.Header>
          </Card.Content>
          <Card.Content>
            <Card.Header>Todo Item</Card.Header>
            <Card.Description>
              <Input
                focus
                placeholder="New Task..."
                onChange={this.onInputTextChange}
                value={this.state.newTaskDescription}
              />
            </Card.Description>
          </Card.Content>
          <Card.Content extra>
            <div className="ui two buttons">
              <Button basic color="green">
                <i className="fa fa-exclamation" />
              </Button>
              <Button basic color="blue">
                <i className="fa fa-plus" />
              </Button>
            </div>
          </Card.Content>
        </Card>
      </form>
    );
  }
}

export default TodoAddForm;
