﻿using System;
using System.Collections.Generic;
using VDS.RDF.Graphs;
using VDS.RDF.Nodes;

namespace VDS.RDF.Query.Paths
{
    /// <summary>
    /// Represents a triple or a path
    /// </summary>
    public class TriplePath
        : IEquatable<TriplePath>
    {

        public TriplePath(Triple t)
            : this(t.Subject, new Property(t.Predicate), t.Object) { }

        public TriplePath(INode subj, IPath path, INode obj)
        {
            if (subj == null) throw new ArgumentNullException("subj");
            if (path == null) throw new ArgumentNullException("path");
            if (obj == null) throw new ArgumentNullException("obj");
            this.Subject = subj;
            this.Path = path;
            this.Object = obj;

            List<String> vars = new List<string>();
            if (subj.NodeType == NodeType.Variable) vars.Add(subj.VariableName);
            if (!this.IsPath)
            {
                if (((Property)this.Path).Predicate.NodeType == NodeType.Variable) vars.Add(((Property)this.Path).Predicate.VariableName);
            }
            if (obj.NodeType == NodeType.Variable) vars.Add(obj.VariableName);
            this.Variables = vars.AsReadOnly();
        }

        /// <summary>
        /// Gets whether this is a simple triple
        /// </summary>
        public bool IsTriple { get { return this.Path is Property; } }

        /// <summary>
        /// Gets whether this is a complex path
        /// </summary>
        public bool IsPath { get { return !this.IsTriple; } }

        public INode Subject { get; private set; }

        public INode Object { get; private set; }

        public IPath Path { get; private set; }

        public IEnumerable<String> Variables { get; private set; }

        /// <summary>
        /// Converts into a simple triple if possible
        /// </summary>
        /// <returns>Simple triple</returns>
        /// <exception cref="RdfException">Thrown if the path is not a simple triple</exception>
        public Triple AsTriple()
        {
            if (this.IsTriple) return new Triple(this.Subject, ((Property)this.Path).Predicate, this.Object);
            throw new RdfException("A path cannot be converted to a triple");
        }

        public bool Equals(TriplePath other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;

            return this.Subject.Equals(other.Subject) && this.Object.Equals(other.Object) && this.Path.Equals(other.Path);
        }
    }
}
