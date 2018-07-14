import React from "react";
import propTypes from "prop-types";
import Commentary from "../Commentary/Commentary.jsx";
import ApiService from "../../services/ApiService";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import modelUtils from "../../utils/ModelUtils";
import TextField from 'material-ui/TextField';
import FlatButton from 'material-ui/FlatButton';
import "./CommentaryList.css";
import constants from "../../utils/Constants";

class CommentaryList extends React.PureComponent {
    constructor() {
        super();

        this.state = {
            comments: []
        };

        this.noteSchema = modelSchemaProvider.getSchemaByName("Note");
        this.noteCommentSchema = modelSchemaProvider.getSchemaByName("NoteComment");
    }

    componentWillMount() {
        this._loadComments();
    }

    render() {
        if (!this.props.noteId || this.props.noteId === constants.EMPTY_GUID) {
            return null;
        }

        return (
            <div>
                <span>Комментарии</span>
                <div className="comments-box">
                    {this.state.comments.map(comment => (
                        <div className="comment-wrapper" key={modelUtils.getPrimaryValue(comment, modelSchemaProvider.getSchemaByName("NoteComment"))}>
                            <Commentary date={comment.modifiedOn} text={comment.text}/>
                        </div>
                    ))}
                </div>
                <div>
                    <TextField hintText="Комментарий" multiLine={true} rows={2} ref="commentTextBox"/>
                    <FlatButton label="Добавить" onClick={this._onAddButtonClick}/>
                </div>
            </div>
        );
    }

    _loadComments() {
        new ApiService(this.noteSchema.resourceName).getItems(this.props.noteId, null, this.noteCommentSchema.resourceName)
            .then(response => this.setState({
                comments: response.data
            }));
    }

    _addNewComment(text) {
        new ApiService(this.noteCommentSchema.resourceName).addItem({
            text: text,
            noteId: this.props.noteId
        })
        .then(response => {
            this.refs.commentTextBox.input.setValue("");
            this._loadComments();
        });
    }

    _onAddButtonClick = () => {
        if (this.refs.commentTextBox.getValue()) {
            this._addNewComment(this.refs.commentTextBox.getValue());
        }
    }
}

CommentaryList.propTypes = {
    noteId: propTypes.string.isRequired
};

export default CommentaryList;