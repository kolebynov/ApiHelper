import React from "react";
import BaseModelSchemaPage from "../BaseModelSchemaPage/BaseModelSchemaPage.jsx";
import CommentaryList from "../../CommentaryList/CommentaryList.jsx";
import "./NoteModelPage.css";
import FileList from "../../FileList/FileList.jsx";

class NoteModelPage extends BaseModelSchemaPage {
    _renderBody() {
        return (
            <div className="note-body">
                <div className="note-body-left">
                    {this.renderEditComponent("name")}
                    {this.renderEditComponent("description")}
                    {this.renderEditComponent("category")}
                    <div className="commentaries-wrapper">
                        <CommentaryList noteId={this.props.primaryColumnValue} />
                    </div>
                </div>
                <div className="files-wrapper">
                    <FileList noteId={this.props.primaryColumnValue} />
                </div>
            </div>
        );
    }
}

export default NoteModelPage;