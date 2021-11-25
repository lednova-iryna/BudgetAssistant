import React, { useState } from "react";
import LoginIconBtn from "./LoginIconBtn";
import LoginSwitchBtn from "./LoginSwitchBtn";

const LoginControl = () => {
  const [auth, setAuth] = useState(true);
  const handleChange = (event) => {
    setAuth(event.target.checked);
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        flexDirection: "row",
        minHeight: "50px",
        margin: "0 15px ",
      }}
    >
      <LoginSwitchBtn checked={auth} onChange={handleChange} />
      {auth && <LoginIconBtn />}
    </div>
  );
};

export default LoginControl;
