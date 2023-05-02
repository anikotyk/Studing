import './App.css';
import LoginButton from "./components/LoginButton";
import LogoutButton from "./components/LogoutButton";
import {useAuth0} from "@auth0/auth0-react";
import PageContent from "./components/PageContent";
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const {isLoading} = useAuth0();
    if(isLoading) return <div>Loading...</div>

    return (
      <div id = "app">
          <div id="appContent">
              <div id = "logButton">
                  <LoginButton />
                  <LogoutButton />
              </div>
              <div id="pageContent">
                  <PageContent />
              </div>
          </div>
      </div>
    );
}

export default App;
