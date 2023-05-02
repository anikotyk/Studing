import './App.css';
import LoginButton from "./components/LoginButton";
import LogoutButton from "./components/LogoutButton";
import {useAuth0} from "@auth0/auth0-react";
import PageContent from "./components/PageContent";
import 'bootstrap/dist/css/bootstrap.min.css';
import LoadingComponent from "./components/LoadingComponent";
import LoginPage from "./components/LoginPage";

function App() {
    const { user, isAuthenticated , isLoading} = useAuth0();
    if(isLoading) return <LoadingComponent/>
    if(!isAuthenticated) return <LoginPage/>

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
