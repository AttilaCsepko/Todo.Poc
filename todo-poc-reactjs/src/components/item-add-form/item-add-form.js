import { Component } from 'react';
import './item-add-form.css';

class ItemAddForm extends Component {

    state = {
        inpuText: '',
        important: false,
    };

    onInputTextChange = (e) => {
        this.setState({
            inpuText: e.target.value
        });
      };

    onSubmit = (e) => {
        e.preventDefault();

        const { inpuText } = this.state;
        if (inpuText.length === 0) {
            return;
        }

        const { onAdded } = this.props;
        onAdded(
            this.state.inpuText,
            this.state.important
        );
       
        this.setState({ 
            inpuText: '', 
            important:false 
        });
    };

    toggleImportant = (e) => {
        e.preventDefault();
        this.setState((state)=>{
            return{
                important: !state.important
            };
        });
    };

    render() {
        
        const styleImportantButton = {
            backgroundColor: this.state.important ? '#28a745': '#fff',
            color: this.state.important ? '#fff' : '#28a745'
          };

        return (
            <form
                className="item-add-form d-flex"
                onSubmit={this.onSubmit}>
                <input
                    className="form-control"
                    onChange={this.onInputTextChange}
                    value={this.state.inpuText}
                    placeholder="type the new task" />
                <button type="button"
                    className="btn btn-outline-success btn-sm float-right"
                    style ={styleImportantButton}
                    onClick={this.toggleImportant}>
                    <i className="fa fa-exclamation" />
                </button>                    
                <button 
                    className="btn btn-outline-secondary btn-sm float-right"
                    type="submit">
                    <i className="fa fa-plus" />
                </button>
            </form>
        );
    };

}

export default ItemAddForm;