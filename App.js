import { useContext, useEffect,useState } from "react";
import Footer from "./components/Footer";
import Header from "./components/Header";
import Home from "./components/Home";
import SideNav from "./components/SideNav";
import { userContext } from "./views/context/UserProvider";
import { useNavigate } from "react-router-dom";

function App() {
  const {user} = useContext(userContext);
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState();

useEffect(()=>{
  if(user === null){
    navigate("/Login");
  }else{
    setUsuario(JSON.parse(user).nombre);
  }
},[user,navigate])
  return (
    <>
      <Header />
      <Home />
      <SideNav />
      <Footer />
    </>
  );
}

export default App;
