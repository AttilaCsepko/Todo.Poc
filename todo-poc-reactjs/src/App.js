import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Todos from './components/todos'

const App = () => {
  return (
    <Router>
      <Switch>
        <Route path="/">
          <Todos />
        </Route>
      </Switch>
    </Router>
  );

}

export default App;
