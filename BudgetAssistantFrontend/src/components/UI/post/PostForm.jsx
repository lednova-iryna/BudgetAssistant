import { FormControl } from "@mui/material";
import React, { useContext, useState } from "react";
import { PostContext } from "../../../contexts/context";
import BABtn from "../button/BA-Btn";
import BAToggleBtn from "../button/BA-ToggleBtn";
import BADatePicker from "../input/BA-DatePicker";
import BAInput from "../input/BA-Input";
import BASelect from "../input/BA-Select";

const PostForm = () => {
  const { posts, setPost } = useContext(PostContext);
  const [date, setDate] = useState(new Date());
  const [category, setCategory] = useState("");
  const [description, setDescription] = useState("");
  const [amount, setAmount] = useState("");
  const postTypes = ["income", "expenses"];
  const [postType, setPostType] = useState("");

  const addNewPost = (e) => {
    e.preventDefault();
    const newPost = {
      id: Date.now(),
      date,
      category,
      description,
      amount,
      postType,
    };

    setPost([...posts, newPost]);
  };

  return (
    <div>
      <FormControl
        sx={{
          m: 2,
          display: "flex",
          justifyContent: "center",
          flexDirection: "row",
        }}
      >
        <BADatePicker value={date} onChange={(newValue) => setDate(newValue)} />
        <BASelect
          //    flex-direction: initial;
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          label="Category"
          options={[
            { value: 1, name: "Flat rent" },
            { value: 2, name: "Car costs" },
            { value: 3, name: "Health costs" },
          ]}
        />
        <BAInput
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          type="text"
          label="Description"
        />
        <BAInput
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          type="text"
          label="Amount"
        />
        <BAToggleBtn
          keyProp="create-post-type-toggle"
          values={postTypes}
          currentValue={postType}
          onChange={(e, selectedType) => setPostType(selectedType)}
        />
      </FormControl>
      <BABtn variant="outlined" onClick={addNewPost}>
        Add
      </BABtn>
    </div>
  );
};

export default PostForm;
