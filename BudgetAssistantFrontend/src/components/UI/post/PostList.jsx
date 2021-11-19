import React, { useContext } from "react";
import { PostContext } from "../../../contexts/context";
import PostItem from "./PostItem";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import { makeStyles, styled } from "@mui/styles";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import { useTableStyles } from "../../../hooks/useTableStyle";
import PostHeader from "./PostHeader";

const PostList = () => {
  const { posts, setPost } = useContext(PostContext);
  const classes = useTableStyles();

  // const StyledTableHeaderCell = styled(TableCell)(({ theme }) => ({
  //   [`&.${tableCellClasses.head}`]: {
  //     backgroundColor: theme.palette.common.black,
  //     color: theme.palette.common.white,
  //   },
  //   [`&.${tableCellClasses.body}`]: {
  //     fontSize: 14,
  //     // align: center,
  //     fontWeight: "bold",
  //   },
  // }));

  if (!posts.length) {
    return (
      <TableContainer className={classes.tableContainer} component={Paper} >
        <Table className={classes.table} aria-label="customized table">
          <PostHeader />
          <TableCell colspan="6" style={{ textAlign: "center" }}>
            {" "}
            There are no entries here!
          </TableCell>
        </Table>
      </TableContainer>
    );
  }

  return (
    <TableContainer className={classes.tableContainer} component={Paper}>
      <Table className={classes.table} aria-label="customized table">
        <PostHeader />
        <TableBody>
          {posts && posts.map((post) => <PostItem post={post} key={post.id} />)}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default PostList;
